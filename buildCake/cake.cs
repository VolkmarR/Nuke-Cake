var target = Argument("target", "Compile");
var configuration = Argument("configuration", "Release");

var solution = "./NukeCakeTests.sln";
var srcDirectory = "./src";
var publishPath = "./publish";
var artefactsPath = "./artefacts";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
    {
        CleanDirectories($"{srcDirectory}/**/obj");
        CleanDirectories($"{srcDirectory}/**/bin");
        CleanDirectory(publishPath);
        CleanDirectory(artefactsPath);
    });

Task("Restore")
    .Does(() =>
    {
        DotNetRestore(solution, new DotNetRestoreSettings
        {
            Verbosity = DotNetVerbosity.Quiet,
        });
    });

Task("Compile")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        DotNetBuild(solution, new DotNetBuildSettings
        {
            Configuration = configuration,
            NoRestore = true,
            Verbosity = DotNetVerbosity.Quiet,
        });
    });

Task("Test")
    .IsDependentOn("Compile")
    .Does(() =>
    {
        DotNetTest(solution, new DotNetTestSettings
        {
            Configuration = configuration,
            Verbosity = DotNetVerbosity.Quiet,
        });
    });

Task("Publish")
    .IsDependentOn("Test")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetPublish($"{srcDirectory}/Md2Html/Md2Html.csproj", new DotNetPublishSettings
        {
            Configuration = "Release",
            OutputDirectory = publishPath,
        });

        Zip(publishPath, $"{artefactsPath}/Md2Html.zip");
    });

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);