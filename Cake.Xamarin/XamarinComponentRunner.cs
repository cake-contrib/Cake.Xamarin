using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Xamarin
{    
    internal class XamarinComponentRunner : Tool<XamarinComponentSettings>
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

            Run (settings, builder, settings.ToolPath, new ProcessSettings { RedirectStandardOutput = true }, null);
        }

        public void Package (DirectoryPath componentYamlDirectory, XamarinComponentSettings settings)
        {
            var builder = new ProcessArgumentBuilder ();

            builder.Append ("package");
            builder.AppendQuoted (componentYamlDirectory.MakeAbsolute (_cakeEnvironment).FullPath);

            Run (settings, builder, settings.ToolPath, new ProcessSettings { RedirectStandardOutput = true }, null);
        }
    }
}
