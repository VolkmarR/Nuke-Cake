var isLocalBuild = BuildSystem.IsLocalBuild;
var target = Argument("target", "Compile");
var configuration = Argument("configuration", isLocalBuild ? "Debug" : "Release");

var solution = "./NukeCakeTests.sln";
var srcDirectory = "./src";
var publishPath = "./publish";
var artefactsPath = "./artefacts";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

var cleanTask = Task("Clean")
    .Does(() =>
    {
        CleanDirectories($"{srcDirectory}/**/obj");
        CleanDirectories($"{srcDirectory}/**/bin");
        CleanDirectory(publishPath);
        CleanDirectory(artefactsPath);
    });

var restoreTask = Task("Restore")
    .Does(() =>
    {
        DotNetRestore(solution, new DotNetRestoreSettings
        {
            Verbosity = DotNetVerbosity.Minimal,
        });
    });

var compileTask = Task("Compile")
    .IsDependentOn(restoreTask)
    .Does(() =>
    {
        DotNetBuild(solution, new DotNetBuildSettings
        {
            Configuration = configuration,
            NoRestore = true,
            Verbosity = DotNetVerbosity.Minimal,
        });
    });

var testTask = Task("Test")
    .IsDependentOn(compileTask)
    .Does(() =>
    {
        DotNetTest(solution, new DotNetTestSettings
        {
            Configuration = configuration,
            Verbosity = DotNetVerbosity.Minimal,
        });
    });

var publishTask = Task("Publish")
    .IsDependentOn(testTask)
    .IsDependentOn(cleanTask)
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