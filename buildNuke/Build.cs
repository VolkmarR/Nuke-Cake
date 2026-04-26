using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution(GenerateProjects = true)] readonly Solution Solution;

    AbsolutePath ArtefactsPath => Solution.Directory / "artefacts";
    AbsolutePath PublishPath => Solution.Directory / "publish";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            var srcDirectory = RootDirectory / "src";
            srcDirectory.GlobDirectories("**/obj").DeleteDirectories();
            srcDirectory.GlobDirectories("**/bin").DeleteDirectories();
            PublishPath.CreateOrCleanDirectory();
            ArtefactsPath.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution)
                .SetVerbosity(DotNetVerbosity.quiet));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetNoRestore(true)
                .SetVerbosity(DotNetVerbosity.quiet));
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetVerbosity(DotNetVerbosity.quiet));
        });

    Target Publish => _ => _
        .DependsOn(Test)
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetPublish(_ => _
                .SetProject(Solution.Md2Html)
                .SetConfiguration(Configuration.Release)
                .SetOutput(PublishPath));

            PublishPath.CompressTo(ArtefactsPath / "Md2Html.zip");
        });
}