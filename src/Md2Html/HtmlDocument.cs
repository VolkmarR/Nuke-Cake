namespace Md2Html;

/// <summary>
/// Utility for wrapping an HTML fragment in a full HTML document.
/// </summary>
public static class HtmlDocument
{
    /// <summary>
    /// Wraps an HTML body fragment in a complete HTML5 document.
    /// </summary>
    /// <param name="title">The document title.</param>
    /// <param name="bodyContent">The HTML body content (fragment).</param>
    /// <returns>A complete HTML5 document string.</returns>
    public static string Wrap(string title, string bodyContent) =>
        $"""
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <title>{System.Net.WebUtility.HtmlEncode(title)}</title>
        </head>
        <body>
        {bodyContent}
        </body>
        </html>
        """;
}
