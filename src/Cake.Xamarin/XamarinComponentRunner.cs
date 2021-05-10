using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System.Collections.Generic;

namespace Cake.Xamarin
{
    internal class XamarinComponentRunner : Tool<XamarinComponentSettings>
    {
        readonly ICakeEnvironment _cakeEnvironment;

        public XamarinComponentRunner (IFileSystem fileSystem, ICakeEnvironment cakeEnvironment, IProcessRunner processRunner, IToolLocator tools) 
            : base (fileSystem, cakeEnvironment, processRunner, tools)
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

        public void Restore (FilePath projectFile, XamarinComponentRestoreSettings settings)
        {
            var builder = new ProcessArgumentBuilder ();

            builder.Append ("restore");
            builder.AppendQuoted (projectFile.MakeAbsolute (_cakeEnvironment).FullPath);

            if (!string.IsNullOrEmpty (settings.Email))
                builder.Append ("--user={0}", settings.Email);

            if (!string.IsNullOrEmpty (settings.Password))
                builder.Append ("--password={0}", settings.Password);

            Run (settings, builder);
        }

        public void Package (DirectoryPath componentYamlDirectory, XamarinComponentSettings settings)
        {
            var builder = new ProcessArgumentBuilder ();

            builder.Append ("package");
            builder.AppendQuoted (componentYamlDirectory.MakeAbsolute (_cakeEnvironment).FullPath);

            Run (settings, builder);
        }

        public void Upload (FilePath xamComponentFile, XamarinComponentUploadSettings settings)
        {
            var builder = new ProcessArgumentBuilder ();

            builder.Append ("upload");
            builder.AppendQuoted (xamComponentFile.MakeAbsolute (_cakeEnvironment).FullPath);

            if (!string.IsNullOrEmpty (settings.Email))
                builder.Append ("--user={0}", settings.Email);

            if (!string.IsNullOrEmpty (settings.Password))
                builder.Append ("--password={0}", settings.Password);

            Run (settings, builder);
        }

        public void Submit (FilePath xamComponentFile, XamarinComponentSubmitSettings settings)
        {
            var builder = new ProcessArgumentBuilder ();

            builder.Append ("upload");
            builder.AppendQuoted (xamComponentFile.MakeAbsolute (_cakeEnvironment).FullPath);

            if (!string.IsNullOrEmpty (settings.Email))
                builder.Append ("--user={0}", settings.Email);

            if (!string.IsNullOrEmpty (settings.Password))
                builder.Append ("--password={0}", settings.Password);

            Run (settings, builder);
        }
    }
}
