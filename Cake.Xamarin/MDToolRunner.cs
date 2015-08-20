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
