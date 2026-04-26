using System.Reflection;
using Spectre.Console.Cli;

var app = new CommandApp<Md2Html.ConvertCommand>();

app.Configure(config =>
{
    config.SetApplicationName("md2html");
    var version = Assembly.GetEntryAssembly()
                      ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                      ?.InformationalVersion
                  ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString()
                  ?? "0.0.0";
    config.SetApplicationVersion(version);

    config.AddExample("readme.md");
    config.AddExample("readme.md", "output.html");
    config.AddExample("readme.md", "--full-document");
});

return app.Run(args);