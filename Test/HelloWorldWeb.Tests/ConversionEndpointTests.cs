using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace HelloWorldWeb.Tests;

/// <summary>
/// Black-box tests for the GET / endpoint. ConvertNumberToWords/ConvertNumber
/// are private local functions in Program.cs, so they are exercised through
/// the HTTP endpoint rather than called directly. Requires Program.cs to
/// expose `public partial class Program { }` (added once, at the bottom of
/// the file) so WebApplicationFactory<Program> can bootstrap the app.
/// Test IDs (TC-xx) correspond to the test plan document.
/// </summary>
public class ConversionEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string OutOfRangeMessage =
        "That number is out of range, please enter a number between " +
        "-999999999999999999999999999 and 999999999999999999999999999.";

    private static readonly Regex ConvertedNumberRegex = new(
        "<p class=\"converted-number\">(.*?)</p>",
        RegexOptions.Singleline | RegexOptions.Compiled);

    private readonly HttpClient _client;

    public ConversionEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> GetConvertedTextAsync(string? queryValue)
    {
        var url = queryValue is null ? "/" : $"/?number={Uri.EscapeDataString(queryValue)}";
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var html = await response.Content.ReadAsStringAsync();
        var match = ConvertedNumberRegex.Match(html);
        return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
    }

    // TC-01 to TC-14, TC-07
    [Theory]
    [InlineData("123.45", "ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS")]
    [InlineData("0", "ZERO DOLLARS")]
    [InlineData("0.05", "ZERO DOLLARS AND FIVE CENTS")]
    [InlineData("1.00", "ONE DOLLAR")]
    [InlineData("5.01", "FIVE DOLLARS AND ONE CENT")]
    [InlineData("-45.50", "NEGATIVE FORTY-FIVE DOLLARS AND FIFTY CENTS")]
    [InlineData("-0", "ZERO DOLLARS")]
    [InlineData("300", "THREE HUNDRED DOLLARS")]
    [InlineData("115", "ONE HUNDRED AND FIFTEEN DOLLARS")]
    [InlineData("20", "TWENTY DOLLARS")]
    [InlineData("21", "TWENTY-ONE DOLLARS")]
    [InlineData("13", "THIRTEEN DOLLARS")]
    [InlineData("1000000", "ONE MILLION DOLLARS")]
    [InlineData("1234567",
        "ONE MILLION TWO HUNDRED AND THIRTY-FOUR THOUSAND FIVE HUNDRED AND SIXTY-SEVEN DOLLARS")]
    public async Task ConvertsExpectedValues(string input, string expected)
    {
        var actual = await GetConvertedTextAsync(input);
        Assert.Equal(expected, actual);
    }

    // TC-15: exactly at the documented maximum supported value
    [Fact]
    public async Task TC15_MaxSupportedValue_ConvertsFully()
    {
        const string expected =
            "NINE HUNDRED AND NINETY-NINE SEPTILLION " +
            "NINE HUNDRED AND NINETY-NINE SEXTILLION " +
            "NINE HUNDRED AND NINETY-NINE QUINTILLION " +
            "NINE HUNDRED AND NINETY-NINE QUADRILLION " +
            "NINE HUNDRED AND NINETY-NINE TRILLION " +
            "NINE HUNDRED AND NINETY-NINE BILLION " +
            "NINE HUNDRED AND NINETY-NINE MILLION " +
            "NINE HUNDRED AND NINETY-NINE THOUSAND " +
            "NINE HUNDRED AND NINETY-NINE DOLLARS";

        var actual = await GetConvertedTextAsync("999999999999999999999999999");
        Assert.Equal(expected, actual);
    }

    // TC-16 (one above max), TC-17 (non-numeric), TC-18 (malformed), TC-21 (exponent notation)
    [Theory]
    [InlineData("1000000000000000000000000000")]
    [InlineData("abc")]
    [InlineData("12.34.56")]
    [InlineData("1E10")]
    public async Task RejectsOutOfRangeOrInvalidInput(string input)
    {
        var actual = await GetConvertedTextAsync(input);
        Assert.Equal(OutOfRangeMessage, actual);
    }

    // TC-19: empty value
    [Fact]
    public async Task EmptyValue_DoesNotRenderConvertedNumber()
    {
        var response = await _client.GetAsync("/?number=");
        var html = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("class=\"converted-number\"", html);
    }

    // TC-20: parameter omitted entirely
    [Fact]
    public async Task MissingParameter_DoesNotRenderConvertedNumber()
    {
        var response = await _client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("class=\"converted-number\"", html);
    }

    // TC-22: documents known Defect D-2 (rounded cents not carried into dollars).
    // If D-2 is fixed, update the expected value to
    // "ONE HUNDRED AND TWENTY-FOUR DOLLARS" and drop this comment.
    [Fact]
    public async Task TC22_KnownDefect_CentsRoundToOneHundred_NotCarried()
    {
        var actual = await GetConvertedTextAsync("123.999");
        Assert.Equal("ONE HUNDRED AND TWENTY-THREE DOLLARS AND ONE HUNDRED CENTS", actual);
    }

    // TC-23, TC-24: rounding midpoints use banker's rounding (MidpointRounding.ToEven) - see D-3
    [Theory]
    [InlineData("10.015", "TEN DOLLARS AND TWO CENTS")]
    [InlineData("10.025", "TEN DOLLARS AND TWO CENTS")]
    public async Task RoundingMidpoints_UseBankersRounding(string input, string expected)
    {
        var actual = await GetConvertedTextAsync(input);
        Assert.Equal(expected, actual);
    }
}
