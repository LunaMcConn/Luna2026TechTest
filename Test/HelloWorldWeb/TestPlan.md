Test Plan

Goal:
To verify that the program correctly converts a range of numerical dollar amounts into words, handles invalid and out-of-range inputs, and produces the expected output for edge cases like zero, negative numbers, and numbers at the boundary of the support range.

Scope
The test plan covers the GET/ endpoint, which is the only entry point to the app's logic. 
ConvertNumberToWords() and ConvertNumber() are private local functions inside Program.cs, so they can't be called directly from a test project.
Instead, all tests access them indirectly by sending HTTP requests to the running app and reading the converted result back out of the returned HTML.

Test Approach
I used a mix of manual and automated testing. Manual testing was used in the beginning and I switched to automated tests when the main difficulties were specific edge cases where manual checking became tedious. Automating the tests allowed me to re-run all specific cases quickly to see if a change broke or fixed something.
Every test goes through the / endpoint with a number query parameter and checks converted text in the HTML. This tests how the app would actually appear when used, rather than testing internal logic.

Each listed test case has an ID (like TC-xx) which match the test names and comments in the test project

ID, Description, Input, Expected Output
TC-01, example dollars and cents, 123.45, ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS
TC-02, zero, 0, ZERO DOLLARS
TC-03, zero dollars, some cents	0.05, ZERO DOLLARS AND FIVE CENTS
TC-04, Singular "DOLLAR", 1.00, ONE DOLLAR
TC-05, Singular "CENT", 5.01, FIVE DOLLARS AND ONE CENT
TC-06, Negative number, -45.50, NEGATIVE FORTY-FIVE DOLLARS AND FIFTY CENTS
TC-07, Negative zero, -0, ZERO DOLLARS
TC-08, Round hundred, 300, THREE HUNDRED DOLLARS
TC-09, Hundreds with "AND", 115, ONE HUNDRED AND FIFTEEN DOLLARS
TC-10, Twenty, 20, TWENTY DOLLARS
TC-11, twenties with hyphenated ones, 21, TWENTY-ONE DOLLARS
TC-12, Teens, 13, THIRTEEN DOLLARS
TC-13, Round million, 1000000, ONE MILLION DOLLARS
TC-14, Multi-group number, 1234567, ONE MILLION TWO HUNDRED AND THIRTY-FOUR THOUSAND FIVE HUNDRED AND SIXTY-SEVEN DOLLARS
TC-15, Maximum supported value, 999999999999999999999999999, NINE HUNDRED AND NINETY-NINE SEPTILLION NINE HUNDRED AND NINETY-NINE SEXTILLION NINE HUNDRED AND NINETY-NINE QUINTILLION NINE HUNDRED AND NINETY-NINE QUADRILLION NINE HUNDRED AND NINETY-NINE TRILLION NINE HUNDRED AND NINETY-NINE BILLION NINE HUNDRED AND NINETY-NINE MILLION NINE HUNDRED AND NINETY-NINE THOUSAND NINE HUNDRED AND NINETY-NINE DOLLARS
TC-16, One above the maximum, 1000000000000000000000000000, Out-of-range message
TC-17, Non-numeric input, abc, Out-of-range message
TC-18, double decimial number, 12.34.56, Out-of-range message
TC-19, Empty value, ?number=, No converted-number element rendered
TC-20, Parameter omitted entirely, /, no converted number  rendered
TC-21, Exponent notation, 1E10, Out-of-range message
TC-22, Known defect, 123.999, ONE HUNDRED AND TWENTY-THREE DOLLARS AND ONE HUNDRED CENTS 
TC-23, Rounding number, 10.015, TEN DOLLARS AND TWO CENTS
TC-24, Rounding number 2, 10.025, TEN DOLLARS AND TWO CENTS


How to run:
To run the test naviagte to the test directory (HelloWorldWeb.Tests) and then enter "dotnet test"