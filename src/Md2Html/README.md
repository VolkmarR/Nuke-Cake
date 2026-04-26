# Md2Html

A simple, testable Markdown to HTML converter built with .NET 10, Markdig, and Spectre.Console.

## Features

- **Markdig Integration**: Uses the powerful Markdig engine with advanced extensions enabled (tables, attributes, auto-identifiers, etc.).
- **Spectre.Console CLI**: A beautiful command-line interface with progress spinners and helpful error messages.
- **Full Document Support**: Option to wrap HTML fragments in a standard HTML5 boilerplate.
- **Unit Tested**: Core logic is isolated and covered by a comprehensive test suite.

## Installation & Building

Ensure you have the .NET 10 SDK installed.

```powershell
dotnet build src/Md2Html/Md2Html.csproj
```

## Usage

Run the tool using `dotnet run`:

### Basic Conversion
Converts `input.md` to `input.html`:
```powershell
dotnet run --project src/Md2Html -- input.md
```

### Custom Output Path
```powershell
dotnet run --project src/Md2Html -- input.md custom-output.html
```

### Full HTML Document
Wraps the output in `<html>`, `<head>`, and `<body>` tags:
```powershell
dotnet run --project src/Md2Html -- input.md --full-document
```

## Running Tests

The project includes xUnit tests for the conversion logic and HTML wrapping:

```powershell
dotnet test src/Md2Html.Tests
```

## Dependencies

- [Markdig](https://github.com/lunet-io/markdig) - Markdown processing.
- [Spectre.Console.Cli](https://github.com/spectreconsole/spectre.console) - CLI argument parsing and rendering.
