using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

/// <summary>
/// Handle GET request from webpage and check for "number" 
/// If priovided, convert the number into words and display on webpage
/// </summary>
app.MapGet("/", async context =>
{
    var query = context.Request.Query;
string numberValue = query["number"].ToString();

// Load base HTML file
string html = File.ReadAllText(Path.Combine(app.Environment.ContentRootPath, "wwwroot", "index.html"));

// If a number is entered convert it to words and display on screen
if (!string.IsNullOrEmpty(numberValue))
{
    string convertedWords = ConvertNumberToWords(numberValue);
    string display = $"<p class=\"converted-number\">{convertedWords}</p>";
        html = html.Replace("<!-- Converted number appears here :) -->", display);
    }

    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(html);
});

app.Run();

/// <summary>
/// Convert the inputted string ("number") to words with dollars and cents
/// Handles negative numbers as well as numbers outside of the range
/// </summary>
static string ConvertNumberToWords(string input)
{
    try
    {
        // Ensure input is a valid decimal
        if (!decimal.TryParse(input, out decimal number))
        {
            return "That number is out of range, please enter a number between -999999999999999999999999999 and 999999999999999999999999999.";
        }

        // Check if number exceeds supported range
        decimal maxSupported = 999999999999999999999999999m;
        if (Math.Abs(number) > maxSupported)
        {
            return "That number is out of range, please enter a number between -999999999999999999999999999 and 999999999999999999999999999.";
        }

        bool isNegative = number < 0;
        decimal absoluteNumber = Math.Abs(number);
        
        // Handle case if 0 is entered
        if (absoluteNumber == 0)
        {
            return "ZERO DOLLARS";
        }

        // Seperate string into dollars and cents
        decimal dollarDecimal = Math.Floor(absoluteNumber);
        decimal centDecimal = Math.Round((absoluteNumber - dollarDecimal) * 100, 0);

        StringBuilder result = new StringBuilder();

        // Add negative prefix if needed
        if (isNegative)
        {
            result.Append("NEGATIVE ");
        }

        // Convert the dollars
        if (dollarDecimal > 0)
        {
            result.Append(ConvertNumber(dollarDecimal));
            result.Append(dollarDecimal == 1 ? " DOLLAR" : " DOLLARS");
        }
        else
        {
            result.Append("ZERO DOLLARS");
        }

        // Convert the cents
        if (centDecimal > 0)
        {
            result.Append(" AND ");
            result.Append(ConvertNumber(centDecimal));
            result.Append(centDecimal == 1 ? " CENT" : " CENTS");
        }

        return result.ToString();
    }
    catch
    {
        return "Error converting number";
    }
}


/// <summary>
/// Convert the given string ("number") to words
/// Handles ones, tens, hundreds, and thousands
/// </summary>
static string ConvertNumber(decimal number)
{
    if (number == 0) return "ZERO";

    // 1-19 array
    string[] ones = {
        "", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE",
        "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN",
        "SEVENTEEN", "EIGHTEEN", "NINETEEN"
    };

    // Tens array
    string[] tens = {
        "", "", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"
    };

    // Thousands array
    string[] thousands = {
        "", "THOUSAND", "MILLION", "BILLION", "TRILLION", "QUADRILLION", "QUINTILLION",
        "SEXTILLION", "SEPTILLION", "OCTILLION", "NONILLION", "DECILLION"
    };

    number = Math.Abs(number);
    List<string> parts = new List<string>();
    int groupIndex = 0;

    // Break number down into easier groups
    while (number > 0 && groupIndex < thousands.Length)
    {
        int group = (int)(number % 1000);
        if (group > 0)
        {
            StringBuilder groupWords = new StringBuilder();

            // Handle hundreds
            int hundreds = group / 100;
            if (hundreds > 0)
            {
                groupWords.Append(ones[hundreds] + " HUNDRED");
                group %= 100;
                if (group > 0)
                {
                    groupWords.Append(" AND ");
                }
            }

            // Handle tens and ones
            if (group >= 20)
            {
                int tensDigit = group / 10;
                int onesDigit = group % 10;
                groupWords.Append(tens[tensDigit]);
                if (onesDigit > 0)
                {
                    groupWords.Append("-" + ones[onesDigit]);
                }
            }
            else if (group > 0)
            {
                groupWords.Append(ones[group]);
            }

            // Handles the thousands
            if (groupIndex > 0)
            {
                groupWords.Append(" " + thousands[groupIndex]);
            }

            parts.Insert(0, groupWords.ToString());
        }
        number = Math.Floor(number / 1000);
        groupIndex++;
    }

    // Handles out of range numbers
    if (number > 0)
    {
        return "That number is out of range, please enter a number between -999999999999999999999999999 and 999999999999999999999999999.";
    }

    return string.Join(" ", parts);
}

//This allows tests to run
public partial class Program { }