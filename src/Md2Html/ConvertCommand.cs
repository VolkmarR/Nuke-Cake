using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Md2Html;

/// <summary>
/// Settings for the convert command.
/// </summary>
public sealed class ConvertSettings : CommandSettings
{
    [Description("Path to the input Markdown file.")]
    [CommandArgument(0, "<input>")]
    public string Input { get; init; } = string.Empty;

    [Description("Path to the output HTML file. Defaults to <input>.html")]
    [CommandArgument(1, "[output]")]
    public string? Output { get; init; }

    [Description("Wrap the HTML fragment in a full HTML document.")]
    [CommandOption("-f|--full-document")]
    public bool FullDocument { get; init; }
}

/// <summary>
/// Spectre.Console.Cli command that performs the markdown-to-html conversion.
/// </summary>
public sealed class ConvertCommand : Command<ConvertSettings>
{
    protected override int Execute(CommandContext context, ConvertSettings settings, CancellationToken cancellationToken)
    {
        var inputPath = settings.Input;

        if (!File.Exists(inputPath))
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] File not found: {inputPath}");
            return 1;
        }

        var outputPath = settings.Output
            ?? Path.ChangeExtension(inputPath, ".html");

        AnsiConsole.Status()
            .Start("Converting...", ctx =>
            {
                ctx.Spinner(Spinner.Known.Dots);
                var converter = new MarkdownConverter();

                if (settings.FullDocument)
                {
                    var markdown = File.ReadAllText(inputPath);
                    var body = converter.Convert(markdown);
                    var title = Path.GetFileNameWithoutExtension(inputPath);
                    var html = HtmlDocument.Wrap(title, body);
                    File.WriteAllText(outputPath, html);
                }
                else
                {
                    converter.ConvertFile(inputPath, outputPath);
                }
            });

        AnsiConsole.MarkupLine($"[green]Done![/] Output written to: {outputPath}");
        return 0;
    }
}
