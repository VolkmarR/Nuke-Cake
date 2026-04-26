using Markdig;

namespace Md2Html;

/// <summary>
/// Converts Markdown text to HTML using the Markdig pipeline.
/// Isolated from CLI concerns so it can be unit-tested directly.
/// </summary>
public class MarkdownConverter
{
    private readonly MarkdownPipeline _pipeline;

    /// <summary>
    /// Creates a converter with the default Markdig pipeline
    /// (advanced extensions enabled).
    /// </summary>
    public MarkdownConverter() : this(BuildDefaultPipeline()) { }

    /// <summary>
    /// Creates a converter with a custom pipeline (useful for testing).
    /// </summary>
    public MarkdownConverter(MarkdownPipeline pipeline)
    {
        _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
    }

    /// <summary>
    /// Converts a Markdown string to an HTML fragment.
    /// </summary>
    /// <param name="markdown">The Markdown source text.</param>
    /// <returns>The resulting HTML string.</returns>
    public string Convert(string markdown)
    {
        ArgumentNullException.ThrowIfNull(markdown);
        return Markdown.ToHtml(markdown, _pipeline);
    }

    /// <summary>
    /// Converts a Markdown file to an HTML file.
    /// </summary>
    /// <param name="inputPath">Path to the source .md file.</param>
    /// <param name="outputPath">Path where the .html file will be written.</param>
    public void ConvertFile(string inputPath, string outputPath)
    {
        ArgumentNullException.ThrowIfNull(inputPath);
        ArgumentNullException.ThrowIfNull(outputPath);

        inputPath = Path.GetFullPath(inputPath);
        outputPath = Path.GetFullPath(outputPath);

        if (string.Equals(inputPath, outputPath, StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Input and output paths cannot be identical.");
        }

        var markdown = File.ReadAllText(inputPath);
        var html = Convert(markdown);

        var outDir = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrEmpty(outDir))
        {
            Directory.CreateDirectory(outDir);
        }

        File.WriteAllText(outputPath, html);
    }

    private static MarkdownPipeline BuildDefaultPipeline() =>
        new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
}
