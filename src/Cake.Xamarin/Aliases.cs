using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.XPath;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Common.IO;
using Cake.Common.Tools;
using Cake.Common.Tools.NUnit;
using Cake.Common.Diagnostics;
using Cake.Common.Tools.MSBuild;
using System.Xml;
using System.Collections.Generic;

namespace Cake.Xamarin
{
    /// <summary>
    /// Xamarin related cake aliases.
    /// </summary>
    [CakeAliasCategory("Xamarin")]
    public static class XamarinAliases
    {
        internal const string DEFAULT_MDTOOL_PATH = "/Applications/Xamarin Studio.app/Contents/MacOS/mdtool";
        internal const string DEFAULT_VSTOOL_PATH = "/Applications/Visual Studio.app/Contents/MacOS/vstool";

        /// <summary>
        /// DEPRECATED: Creates an android .APK package file
        /// Use the MSBuild target `SignAndroidPackage` instead.
        /// See documentation for more info: https://developer.xamarin.com/guides/android/under_the_hood/build_process/#Build_Targets
        /// </summary>
        /// <returns>The file path of the .APK which was created (all subfolders of the project file specified are searched for .apk files and the newest one found is returned).</returns>
        /// <param name="context">The context.</param>
        /// <param name="projectFile">The .CSPROJ file to build from.</param>
        /// <param name="sign">Will create a signed .APK file if set to <c>true</c> based on the signing settings in the .CSPROJ, otherwise the .APK will be unsigned.</param>
        /// <param name="configurator">The settings configurator.</param>
        [CakeMethodAlias]
        [Obsolete ("Use MSBuild with the `SignAndroidPackage` or `PackageForAndroid` target instead.  See https://developer.xamarin.com/guides/android/under_the_hood/build_process/#Build_Targets for more information")]
        public static FilePath AndroidPackage (this ICakeContext context, FilePath projectFile, bool sign = false, Action<DotNetBuildSettings> configurator = null)
        {
            var target = sign ? "SignAndroidPackage" : "PackageForAndroid";

            if (!context.FileSystem.Exist (projectFile))
                throw new CakeException ("Project File Not Found: " + projectFile.FullPath);
            
            context.DotNetBuild (projectFile, c => {
                c.Configuration = "Release";        
                c.Targets.Add (target);

				// Pass along configuration to user for further changes
				configurator?.Invoke(c);
			});

            var searchPattern = projectFile.GetDirectory () + (sign ? "/**/*-Signed.apk" : "/**/*.apk");

            // Use the globber to find any .apk files within the tree
            return context.Globber
                .GetFiles (searchPattern)
                .OrderByDescending (f => new FileInfo (f.FullPath).LastWriteTimeUtc)
                .FirstOrDefault ();            
        }

        /// <summary>
        /// DEPRECATED: Creates an archive of an app with MDTool
        /// Change your project settings to output an IPA file, and use the MSBuild to build.
        /// See documentation for more info: https://developer.xamarin.com/guides/ios/deployment,_testing,_and_metrics/app_distribution/ipa_support/#Building_via_the_Command_Line_On_Mac
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solutionFile">The solution file.</param>
        /// <param name="projectName">The name of the project within the solution to archive.</param>
        /// <param name="settings">The mdtool settings.</param>
        [CakeMethodAlias]
		[Obsolete("Use MSBuild instead (and configure your iOS project to generate an ipa file on build.  See https://developer.xamarin.com/guides/ios/deployment,_testing,_and_metrics/app_distribution/ipa_support/#Building_via_the_Command_Line_On_Mac for more information")]
        public static void MDToolArchive (this ICakeContext context, FilePath solutionFile, string projectName, Action<MDToolSettings> settings = null)
        {
            var mds = new MDToolSettings ();

            if (settings != null)
                settings (mds);

            var runner = new MDToolRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Archive (solutionFile, projectName, mds);
        }

        /// <summary>
        /// DEPRECATED: Builds a project with MDTool
        /// Use MSBuild instead.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectOrSolutionFile">The project or solution file.</param>
        /// <param name="settings">The mdtool settings.</param>
        [CakeMethodAlias]
        [Obsolete("Use MSBuild instead.")]
        public static void MDToolBuild (this ICakeContext context, FilePath projectOrSolutionFile, Action<MDToolSettings> settings = null)
        {
            var mds = new MDToolSettings ();

            if (settings != null)
                settings (mds);

            var runner = new MDToolRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Build (projectOrSolutionFile, mds);
        }

        /// <summary>
        /// Gets a runner for invoking the Xamarin Studio Add-in Setup Utility.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>A setup utility runner.</returns>
        [CakePropertyAlias]
		[Obsolete("Use VSToolSetupRunner instead.")]
        public static MDToolSetupRunner MDToolSetup (this ICakeContext context)
        {
            var runner = new MDToolSetupRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            return runner;
        }

		/// <summary>
		/// Gets a runner for invoking the Visual Studio Mac Add-in Setup Utility.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>A setup utility runner.</returns>
		[CakePropertyAlias]
		public static VSToolSetupRunner VSToolSetup(this ICakeContext context)
		{
			var runner = new VSToolSetupRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
			return runner;
		}

        /// <summary>
        /// Restores Xamarin Components for a given project
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solutionFile">The project file.</param>
        /// <param name="settings">The xamarin-component.exe tool settings.</param>
        [CakeMethodAlias]
        public static void RestoreComponents (this ICakeContext context, FilePath solutionFile, XamarinComponentRestoreSettings settings = null)
        {
            var runner = new XamarinComponentRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Restore (solutionFile, settings ?? new XamarinComponentRestoreSettings ());
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
            var runner = new XamarinComponentRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Package (componentYamlDirectory, settings ?? new XamarinComponentSettings ());
        }

        /// <summary>
        /// Finds and Uploads .xam component packages which match the globbing patterns
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="xamFileGlobbingPatterns">The globbing patterns to find .xam component package files with.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void UploadComponents (this ICakeContext context, XamarinComponentUploadSettings settings, params string[] xamFileGlobbingPatterns)
        {
            foreach (var pattern in xamFileGlobbingPatterns)
            {
                var files = context.GetFiles(pattern);
                if (files == null || !files.Any())
                    continue;

                foreach (var file in files)
                {
                    UploadComponent(context, file, settings);
                }
            }
        }

        /// <summary>
        /// Uploads a .xam component package which is a new version of an existing component in the store
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="xamComponentPackage">The .xam component package file.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void UploadComponent (this ICakeContext context, FilePath xamComponentPackage, XamarinComponentUploadSettings settings = null)
        {
            var runner = new XamarinComponentRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            int attempts = 0;
            bool success = false;

            while (attempts < settings.MaxAttempts)
            {
                attempts++;
                try
                {
                    runner.Upload (xamComponentPackage, settings ?? new XamarinComponentUploadSettings ());
                    success = true;
                    break;
                }
                catch 
                {
                    context.Warning("Component Upload failed attempt #{0} of {1}", attempts, settings.MaxAttempts);
                }
            }

            if (!success)
            {
                context.Error("Failed to upload {0}", "component");
                throw new Exception("Failed to upload component");
            }
        }

        /// <summary>
        /// Finds and Submits .xam component packages which match the globbing patterns
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="xamFileGlobbingPatterns">The globbing patterns to find .xam component package files with.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void SubmitComponents (this ICakeContext context, XamarinComponentSubmitSettings settings, params string[] xamFileGlobbingPatterns)
        {
            foreach (var pattern in xamFileGlobbingPatterns)
            {
                var files = context.GetFiles(pattern);
                if (files == null || !files.Any())
                    continue;

                foreach (var file in files)
                {
                    SubmitComponent(context, file, settings);
                }
            }
        }

        /// <summary>
        /// Submits a .xam component package which is a brand new component on the store and has no previous versions
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="xamComponentPackage">The .xam component package file.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void SubmitComponent (this ICakeContext context, FilePath xamComponentPackage, XamarinComponentSubmitSettings settings = null)
        {
            var runner = new XamarinComponentRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);

            int attempts = 0;
            bool success = false;

            while (attempts < settings.MaxAttempts)
            {
                attempts++;
                try
                {
                    runner.Submit (xamComponentPackage, settings ?? new XamarinComponentSubmitSettings ());
                    success = true;
                    break;
                }
                catch 
                {
                    context.Warning("Component Submit failed attempt #{0} of {1}", attempts, settings.MaxAttempts);
                }
            }

            if (!success)
            {
                context.Error("Failed to submit {0}", "component");
                throw new Exception("Failed to submit component");
            }
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
            var runner = new TestCloudRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run (apkFile, apiKey, devicesHash, userEmail, uitestsAssemblies, settings ?? new TestCloudSettings ());
        }
    }

	/// <summary>
	/// Common project type GUID values for Xamarin projects.
	/// </summary>
	public static class CommonProjectTypeGuids
	{
		public const string XamarinAndroid 			= "{EFBA0AD7-5A72-4C68-AF49-83D382785DCF}";
		public const string XamarinAndroidBinding 	= "{10368E6C-D01B-4462-8E8B-01FC667A7035}";

		public const string XamariniOS 				= "{FEACFBD2-3405-455C-9665-78FE426C6842}";
		public const string XamariniOSBinding 		= "{8FFB629D-F513-41CE-95D2-7ECE97B6EEEC}";
	}
}
