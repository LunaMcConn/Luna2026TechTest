I chose to use .NET 9.0 SDK x64 as it provides a lightweight and efficient web server to handle both the static file serving (HTML and CSS) and the dynmaic number conversion (my Program.cs).
I store my HTML and CSS files in the wwwroot folder. This handles the GET requests with query parameter processing.

Request handling uses a single endpoint ("/") to process and serve the static page and number conversion. It extracts the number parameter from the query string and dynamically injects the convert result into the HTML page.

My Program.cs has two functions, ConvertNumberToWords() and ConvertNumber().
The ConvertNumberToWords() handles the formatting of the words, adding NEGATIVE, DOLLARS, and CENTS where necessary.

The ConvertNumber() function is what actually handles converting the given number to words. It converts the decimal number into english words, like: 10 -> TEN

The inputted number is accepted as a string rather than a numeric type like int, long, float, or double. This allows for numbers that would usually exceed standard integer limits to be entered.
It also allowed me to avoid floating-point precision issues that may have arised during production.
The alternate approach of directly using a numeric input type would have limited the range of accepted numbers.

When processing the data I used a decimal type as it allows precise control over cent caluations with Math.Round()
When operating with large numbers, numeric data types can occasionally present errors when rounding or processing.

Using a string also helped when allowing for negative numbers. The program just checks for the negative prefix (-) before conversion, and appends it to the front of the converted text. This allowed for straight forward implemention without complicating the conversion logic unnecessarily.

The program processing numbers in groups of thousands using arrays for storing names.
This makes it easy to extend to higher number ranges, it also separates the digit processing and naming, as well as processing numbers in logical chunks rather than digit by digit.

Using try catch error handling ensures if an error arises the program doesn't break right away, and also informs you when an error has occured. Without it the program may just break without any seamingly logical reason.

For the front end, I ensured the page was fully responsive for all device types. Without that, the page would look unpolished and unprofessional on anything other than a desktop, not something I'd be comfortable putting in front of a customer.

I also designed the UI to share Technology1's aesthetic identity. Using their colour palette and branding style was a deliberate choice to meet the "quality of work" criteria. I aimed to produce something that looks and feels like it belongs alongside Technology1's existing products, rather than a standalone tech-test page. The colours themselves had also already been validated against WCAG accessibility standards during my capstone at Technology1 which was handy.