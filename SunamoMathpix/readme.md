# SunamoMathpix

A .NET library for converting images containing mathematical notation to plain text using the [Mathpix](https://mathpix.com/) OCR API.

## Features

- Converts base64-encoded images with math notation to plain text via Mathpix API
- Optional LaTeX-to-Unicode conversion (use the `SunamoLaTex` package)
- Uses PowerShell to invoke the Mathpix REST API through curl

## Usage

```csharp
var helper = new MathpixHelper("your-app-id", "your-app-key", @"C:\path\to\curl");
string result = helper.Text(base64ImageString, null);
```

## Target Frameworks

`net10.0;net9.0;net8.0`

## Links

- [NuGet](https://www.nuget.org/profiles/sunamo)
- [GitHub](https://github.com/sunamo/PlatformIndependentNuGetPackages)
- [Developer site](https://sunamo.cz)

Request for new features / bug report: [Mail](mailto:radek.jancik@sunamo.cz) or on GitHub
