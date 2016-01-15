using System;
using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Common.Tools;
using Cake.Common.Tools.NUnit;

namespace Cake.Xamarin
{
    /// <summary>
    /// Xamarin related cake aliases.
    /// </summary>
    [CakeAliasCategory("Xamarin")]
    public static class XamarinAliases
    {
        internal const string DEFAULT_MDTOOL_PATH = "/Applications/Xamarin Studio.app/Contents/MacOS/mdtool";

        /// <summary>
        /// Creates an android .APK package file
        /// </summary>
        /// <returns>The file path of the .APK which was created (all subfolders of the project file specified are searched for .apk files and the newest one found is returned).</returns>
        /// <param name="context">The context.</param>
        /// <param name="projectFile">The .CSPROJ file to build from.</param>
        /// <param name="sign">Will create a signed .APK file if set to <c>true</c> based on the signing settings in the .CSPROJ, otherwise the .APK will be unsigned.</param>
        /// <param name="configurator">The settings configurator.</param>
        [CakeMethodAlias]
        public static FilePath AndroidPackage (this ICakeContext context, FilePath projectFile, bool sign = false, Action<DotNetBuildSettings> configurator = null)
        {
            var target = sign ? "SignAndroidPackage" : "PackageForAndroid";

            if (!context.FileSystem.Exist (projectFile))
                throw new CakeException ("Project File Not Found: " + projectFile.FullPath);
            
            context.DotNetBuild (projectFile, c => {
                c.Configuration = "Release";        
                c.Targets.Add (target);

                // Pass along configuration to user for further changes
                if (configurator != null)
                    configurator (c);
            });

            var searchPattern = projectFile.GetDirectory () + (sign ? "/**/*-Signed.apk" : "/**/*.apk");

            // Use the globber to find any .apk files within the tree
            return context.Globber
                .GetFiles (searchPattern)
                .OrderBy (f => new FileInfo (f.FullPath).LastWriteTimeUtc)
                .FirstOrDefault ();            
        }

        /// <summary>
        /// Creates an archive of an app with MDTool
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solutionFile">The solution file.</param>
        /// <param name="projectName">The name of the project within the solution to archive.</param>
        /// <param name="settings">The mdtool settings.</param>
        [CakeMethodAlias]
        public static void MDToolArchive (this ICakeContext context, FilePath solutionFile, string projectName, Action<MDToolSettings> settings = null)
        {
            var mds = new MDToolSettings ();

            if (settings != null)
                settings (mds);

            var runner = new MDToolRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Archive (solutionFile, projectName, mds);
        }

        /// <summary>
        /// Builds a project with MDTool
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectOrSolutionFile">The project or solution file.</param>
        /// <param name="settings">The mdtool settings.</param>
        [CakeMethodAlias]
        public static void MDToolBuild (this ICakeContext context, FilePath projectOrSolutionFile, Action<MDToolSettings> settings = null)
        {
            var mds = new MDToolSettings ();

            if (settings != null)
                settings (mds);

            var runner = new MDToolRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Build (projectOrSolutionFile, mds);
        }

        /// <summary>
        /// Restores Xamarin Components for a given project
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solutionFile">The project file.</param>
        /// <param name="settings">The xamarin-component.exe tool settings.</param>
        [CakeMethodAlias]
        public static void RestoreComponents (this ICakeContext context, FilePath solutionFile, XamarinComponentSettings settings = null)
        {
            var runner = new XamarinComponentRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Restore (solutionFile, settings ?? new XamarinComponentSettings ());
        }

        /// <summary>
        /// Packages the component for a given component YAML configuration file
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="componentYamlDirectory">The directory containing the component.yaml file.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void PackageComponent (this ICakeContext context, DirectoryPath componentYamlDirectory, XamarinComponentSettings settings = null)
        {
            var runner = new XamarinComponentRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Package (componentYamlDirectory, settings ?? new XamarinComponentSettings ());
        }

        /// <summary>
        /// Runs UITests in a given assembly using NUnit
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="testsAssembly">The assembly containing NUnit UITests.</param>
        /// <param name="nunitSettings">The NUnit settings.</param>
        [CakeMethodAlias]
        public static void UITest (this ICakeContext context, FilePath testsAssembly, NUnitSettings nunitSettings = null)
        {            
            // Run UITests via NUnit
            context.NUnit (new [] { testsAssembly }, nunitSettings ?? new NUnitSettings ());
        }

        /// <summary>
        /// Uploads an android .APK package to TestCloud and runs UITests
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="apkFile">The .APK file.</param>
        /// <param name="apiKey">The TestCloud API key.</param>
        /// <param name="devicesHash">The hash of the set of devices to run on.</param>
        /// <param name="userEmail">The user account email address.</param>
        /// <param name="uitestsAssemblies">The directory containing the UITests assemblies.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void TestCloud (this ICakeContext context, FilePath apkFile, string apiKey, string devicesHash, string userEmail, DirectoryPath uitestsAssemblies, TestCloudSettings settings = null)
        {
            var runner = new TestCloudRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run (apkFile, apiKey, devicesHash, userEmail, uitestsAssemblies, settings ?? new TestCloudSettings ());
        }
    }
}
