using Md2Html;

namespace Md2Html.Tests;

public class HtmlDocumentTests
{
    [Fact]
    public void Wrap_ProducesValidHtml5Doctype()
    {
        var result = HtmlDocument.Wrap("Test", "<p>Body</p>");
        Assert.StartsWith("<!DOCTYPE html>", result.TrimStart());
    }

    [Fact]
    public void Wrap_IncludesTitle()
    {
        var result = HtmlDocument.Wrap("My Page", "<p>Body</p>");
        Assert.Contains("<title>My Page</title>", result);
    }

    [Fact]
    public void Wrap_IncludesBodyContent()
    {
        var result = HtmlDocument.Wrap("Title", "<p>Hello</p>");
        Assert.Contains("<p>Hello</p>", result);
    }

    [Fact]
    public void Wrap_EncodesSpecialCharsInTitle()
    {
        var result = HtmlDocument.Wrap("<script>", "<p>x</p>");
        Assert.DoesNotContain("<script>", result.Split("<body>")[0]);
        Assert.Contains("&lt;script&gt;", result);
    }

    [Fact]
    public void Wrap_ContainsCharsetMetaTag()
    {
        var result = HtmlDocument.Wrap("T", "");
        Assert.Contains("charset=\"UTF-8\"", result);
    }
}
