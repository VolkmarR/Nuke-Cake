using Md2Html;

namespace Md2Html.Tests;

public class MarkdownConverterTests
{
    private readonly MarkdownConverter _sut = new();

    // ── Headings ─────────────────────────────────────────────────────────────

    [Fact]
    public void Convert_H1_ProducesH1Tag()
    {
        var result = _sut.Convert("# Hello");
        Assert.Contains("<h1 id=\"hello\">Hello</h1>", result);
    }

    [Theory]
    [InlineData("## Two",   "<h2 id=\"two\">Two</h2>")]
    [InlineData("### Three","<h3 id=\"three\">Three</h3>")]
    public void Convert_Headings_ProduceCorrectTags(string markdown, string expected)
    {
        Assert.Contains(expected, _sut.Convert(markdown));
    }

    // ── Emphasis ──────────────────────────────────────────────────────────────

    [Fact]
    public void Convert_BoldText_ProducesStrongTag()
    {
        var result = _sut.Convert("**bold**");
        Assert.Contains("<strong>bold</strong>", result);
    }

    [Fact]
    public void Convert_ItalicText_ProducesEmTag()
    {
        var result = _sut.Convert("*italic*");
        Assert.Contains("<em>italic</em>", result);
    }

    // ── Paragraphs ────────────────────────────────────────────────────────────

    [Fact]
    public void Convert_PlainText_WrappedInParagraph()
    {
        var result = _sut.Convert("Hello World");
        Assert.Contains("<p>Hello World</p>", result);
    }

    [Fact]
    public void Convert_EmptyString_ReturnsEmptyString()
    {
        var result = _sut.Convert(string.Empty);
        Assert.Equal(string.Empty, result);
    }

    // ── Lists ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Convert_UnorderedList_ProducesUlAndLiTags()
    {
        var result = _sut.Convert("- Item 1\n- Item 2");
        Assert.Contains("<ul>", result);
        Assert.Contains("<li>Item 1</li>", result);
        Assert.Contains("<li>Item 2</li>", result);
    }

    [Fact]
    public void Convert_OrderedList_ProducesOlAndLiTags()
    {
        var result = _sut.Convert("1. First\n2. Second");
        Assert.Contains("<ol>", result);
        Assert.Contains("<li>First</li>", result);
        Assert.Contains("<li>Second</li>", result);
    }

    // ── Code ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Convert_InlineCode_ProducesCodeTag()
    {
        var result = _sut.Convert("`code`");
        Assert.Contains("<code>code</code>", result);
    }

    [Fact]
    public void Convert_FencedCodeBlock_ProducesPreCodeTags()
    {
        var result = _sut.Convert("```\nvar x = 1;\n```");
        Assert.Contains("<pre>", result);
        Assert.Contains("<code>", result);
    }

    // ── Links & Images ────────────────────────────────────────────────────────

    [Fact]
    public void Convert_Link_ProducesAnchorTag()
    {
        var result = _sut.Convert("[Click](https://example.com)");
        Assert.Contains("<a href=\"https://example.com\">Click</a>", result);
    }

    [Fact]
    public void Convert_Image_ProducesImgTag()
    {
        var result = _sut.Convert("![alt](img.png)");
        Assert.Contains("<img", result);
        Assert.Contains("src=\"img.png\"", result);
        Assert.Contains("alt=\"alt\"", result);
    }

    // ── Guard clauses ─────────────────────────────────────────────────────────

    [Fact]
    public void Convert_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.Convert(null!));
    }
}