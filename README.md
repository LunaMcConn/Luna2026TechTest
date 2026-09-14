# Numbers to Words Converter

This is a C# ASP.NET Core web application that converts a dollar amount into it's written word form

## Supporting documentation
Below are links to the relavent supporting documentation:
- The test plan can be found [here](https://github.com/LunaMcConn/Luna2026TechTest/blob/main/Test/HelloWorldWeb/TestPlan.md)
- The design documentation can be found [here](https://github.com/LunaMcConn/Luna2026TechTest/blob/main/Test/HelloWorldWeb/DesignDocument.md)


## Requirements
To build this project, you will need to have:
- .NET 9.0 SDK x64, which you can download [here](https://dotnet.microsoft.com/en-us/download)
- and a web browser

## Setup Steps
1. Extract the files and open your terminal

2. Navigate to the HelloWordlWeb directory, like:
```bash
cd HelloWorldWeb
```

3. Run the app, like:
```bash
dotnet run
```

4. The server should now start, open the link shown in the terminal, or manually type the address into your web browser. This is the usual link, however the port may be different so make sure to use the link printed in the terminal
```
http://localhost:5147/
```

## How to use
1. Enter any dollar amount up to 2 decimals into the input field
2. Click the convert button or press enter
3. The written word version of your amount will appear below.

### Example

**Input:** `123.45`

**Output:** `ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS`


### Notes

- Negative numbers are supported and will be prefixed with "NEGATIVE".

- Supported range is up to 999999999999999999999999999 and down to -999999999999999999999999999. Entering a number outside this range will show an error message asking you to choose a number within range. 