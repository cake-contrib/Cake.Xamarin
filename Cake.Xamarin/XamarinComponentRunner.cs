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

}
