#load nuget:https://www.myget.org/F/cake-contrib/api/v2?package=Cake.Recipe&prerelease

Environment.SetVariableNames();

BuildParameters.SetParameters(context: Context,
                            buildSystem: BuildSystem,
                            sourceDirectoryPath: "./src",
                            title: "Cake.Xamarin",
                            repositoryOwner: "redth",
                            repositoryName: "Cake.Xamarin",
                            appVeyorAccountName: "redth",
                            shouldRunDotNetCorePack: true,
                            shouldRunInspectCode: false,
                            shouldRunDupFinder: false,
                            shouldRunCodecov: false,
                            shouldPostToSlack: false,
                            shouldRunIntegrationTests: false);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context,
                            dupFinderExcludePattern: new string[] {
                                BuildParameters.RootDirectoryPath + "/Cake.Xamarin.Tests/*.cs" },
                            testCoverageFilter: "+[*]* -[xunit.*]* -[Cake.Core]* -[Cake.Testing]* -[*.Tests]* -[FakeItEasy]*",
                            testCoverageExcludeByAttribute: "*.ExcludeFromCodeCoverage*",
                            testCoverageExcludeByFile: "*/*Designer.cs;*/*.g.cs;*/*.g.i.cs;*TestProjects*");
Build.RunDotNetCore();
