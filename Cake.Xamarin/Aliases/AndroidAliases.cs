using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common.Tools;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Xamarin
{
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

            context.DotNetBuild (projectFile, c => {
                c.Configuration = "Release";        
                c.Targets.Add (target);

                // Pass along configuration to user for further changes
                if (configurator != null)
                    configurator (c);
            });

            // Use the globber to find any .apk files within the tree
            return context.Globber
                .GetFiles (sign ? "./**/*-Signed.apk" : "./**/*.apk")
                .OrderBy (f => new FileInfo (f.FullPath).LastWriteTimeUtc)
                .FirstOrDefault ();            
        }

        /// <summary>
        /// Creates an archive of the Xamarin.iOS app
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solutionFile">The solution file.</param>
        /// <param name="projectName">The name of the project within the solution to archive.</param>
        [CakeMethodAlias]
        public static void iOSArchive (this ICakeContext context, FilePath solutionFile, string projectName)
        {
            iOSArchive (context, solutionFile, projectName, new MDToolSettings ());
        }

        /// <summary>
        /// Creates an archive of the Xamarin.iOS app
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solutionFile">The solution file.</param>
        /// <param name="projectName">The name of the project within the solution to archive.</param>
        /// <param name="settings">The mdtool settings.</param>
        [CakeMethodAlias]
        public static void iOSArchive (this ICakeContext context, FilePath solutionFile, string projectName, MDToolSettings settings)
        {
            if (!context.Environment.IsUnix ())
                throw new CakeException ("iOSArchive alias only runs on Mac OSX");

            var runner = new MDToolRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Archive (solutionFile, projectName, settings);
        }

        /// <summary>
        /// Builds a Xamarin.iOS project
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectFile">The project file.</param>
        [CakeMethodAlias]
        public static void iOSBuild (this ICakeContext context, FilePath projectFile)
        {
            iOSBuild (context, projectFile, new MDToolSettings ());
        }

        /// <summary>
        /// Builds a Xamarin.iOS project
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectFile">The project file.</param>
        /// <param name="settings">The mdtool settings.</param>
        [CakeMethodAlias]
        public static void iOSBuild (this ICakeContext context, FilePath projectFile, MDToolSettings settings)
        {
            if (!context.Environment.IsUnix ())
                throw new CakeException ("iOSBuild alias only runs on Mac OSX");
            
            var runner = new MDToolRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Build (projectFile, settings);
        }

        /// <summary>
        /// Restores Xamarin Components for a given project
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectFile">The project file.</param>
        [CakeMethodAlias]
        public static void RestoreComponents (this ICakeContext context, FilePath projectFile)
        {
            RestoreComponents (context, projectFile, new XamarinComponentSettings ());
        }

        /// <summary>
        /// Restores Xamarin Components for a given project
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="projectFile">The project file.</param>
        /// <param name="settings">The xamarin-component.exe tool settings.</param>
        [CakeMethodAlias]
        public static void RestoreComponents (this ICakeContext context, FilePath projectFile, XamarinComponentSettings settings)
        {
            var runner = new XamarinComponentRunner (context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Restore (projectFile, settings);
        }
    }

    public class XamarinComponentSettings
    {
        public FilePath ToolPath { get; set; }
    }

    public class XamarinComponentRunner : Tool<XamarinComponentSettings>
    {
        readonly ICakeEnvironment _cakeEnvironment;

        public XamarinComponentRunner (IFileSystem fileSystem, ICakeEnvironment cakeEnvironment, IProcessRunner processRunner, IGlobber globber) 
            : base (fileSystem, cakeEnvironment, processRunner, globber)
        {
            _cakeEnvironment = cakeEnvironment;
        }

        protected override string GetToolName ()
        {
            return "Xamarin-Component";
        }

        protected override IEnumerable<string> GetToolExecutableNames ()
        {
            return new [] { "xamarin-component.exe" };
        }

        public void Restore (FilePath projectFile, XamarinComponentSettings settings)
        {
            var builder = new ProcessArgumentBuilder ();

            builder.Append ("restore");
            builder.AppendQuoted (projectFile.MakeAbsolute (_cakeEnvironment).FullPath);

            Run (settings, builder, settings.ToolPath);
        }
    }


    public class MDToolSettings
    {
        public MDToolSettings ()
        {
            Configuration = "Debug|iPhoneSimulator";
            Target = "Build";
        }

        public FilePath ToolPath { get; set; }

        public bool IncreaseVerbosity { get; set; }

        public string Configuration { get; set; }

        public string Target { get; set; }
    }

    public class MDToolRunner : Tool<MDToolSettings>
    {
        readonly ICakeEnvironment _cakeEnvironment;

        public MDToolRunner (IFileSystem fileSystem, ICakeEnvironment cakeEnvironment, IProcessRunner processRunner, IGlobber globber) 
            : base (fileSystem, cakeEnvironment, processRunner, globber)
        {
            _cakeEnvironment = cakeEnvironment;
        }

        protected override string GetToolName ()
        {
            return "mdtool";
        }

        protected override IEnumerable<string> GetToolExecutableNames ()
        {
            return new [] { "mdtool" };
        }

        protected override IEnumerable<FilePath> GetAlternativeToolPaths (MDToolSettings settings)
        {
            return new [] { new FilePath (XamarinAliases.DEFAULT_MDTOOL_PATH) };
        }

        public void Build (FilePath projectFile, MDToolSettings settings)
        {
            var builder = new ProcessArgumentBuilder ();

            if (settings.IncreaseVerbosity)
                builder.Append ("-v");
            
            builder.Append ("build");
            builder.Append ("-t:" + settings.Target ?? "Build");
            builder.Append ("-c:\"" + settings.Configuration + "\"");
            builder.AppendQuoted (projectFile.MakeAbsolute (_cakeEnvironment).FullPath);

            Run (settings, builder, settings.ToolPath);
        }

        public void Archive (FilePath solutionFile, string projectName, MDToolSettings settings)
        {
            var builder = new ProcessArgumentBuilder ();

            if (settings.IncreaseVerbosity)
                builder.Append ("-v");

            builder.Append ("archive");

            if (!string.IsNullOrEmpty (projectName))
                builder.Append ("-p:" + projectName);
            
            builder.Append ("-c:\"" + settings.Configuration + "\"");
            builder.AppendQuoted (solutionFile.MakeAbsolute (_cakeEnvironment).FullPath);

            Run (settings, builder, settings.ToolPath);
        }
    }
}
