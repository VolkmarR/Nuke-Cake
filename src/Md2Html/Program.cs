using Spectre.Console.Cli;

var app = new CommandApp<Md2Html.ConvertCommand>();

app.Configure(config =>
{
    config.SetApplicationName("md2html");
    config.SetApplicationVersion("1.0.0");

    config.AddExample("readme.md");
    config.AddExample("readme.md", "output.html");
    config.AddExample("readme.md", "--full-document");
});

return app.Run(args);