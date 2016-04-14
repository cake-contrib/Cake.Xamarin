using System;
using Cake.Core.IO;
using Cake.Core.Utilities;
using Cake.Core;
using System.Collections.Generic;

namespace Cake.Xamarin
{
    /// <summary>
    /// Test cloud settings.
    /// </summary>
    public class TestCloudSettings : Cake.Core.Tooling.ToolSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cake.Xamarin.TestCloudSettings"/> class.
        /// </summary>
        public TestCloudSettings () : base ()
        {
            Series = "master";
            Locale = "en-US";
        }

        /// <summary>
        /// Gets or sets the series to test in the cloud.
        /// </summary>
        /// <value>The series.</value>
        public string Series { get;set; }

        /// <summary>
        /// Gets or sets the locale to test with.
        /// </summary>
        /// <value>The locale.</value>
        public string Locale { get; set; }
    }

    internal class TestCloudRunner : Cake.Core.Tooling.Tool<TestCloudSettings>
    {
        readonly ICakeEnvironment _cakeEnvironment;

        public TestCloudRunner (IFileSystem fileSystem, ICakeEnvironment cakeEnvironment, IProcessRunner processRunner, IGlobber globber) 
            : base (fileSystem, cakeEnvironment, processRunner, globber)
        {
            _cakeEnvironment = cakeEnvironment;
        }

        protected override IEnumerable<string> GetToolExecutableNames ()
        {
            return new [] { "test-cloud.exe" };
        }

        protected override string GetToolName ()
        {
            return "Test Cloud";
        }

        public void Run (FilePath apkFile, string apiKey, string devicesHash, string userEmail, DirectoryPath uitestAssemblies, TestCloudSettings settings)
        {
            var builder = new ProcessArgumentBuilder ();

            builder.Append ("submit");
            builder.AppendQuoted (apkFile.MakeAbsolute (_cakeEnvironment).FullPath);
            builder.AppendQuotedSecret (apiKey);
            builder.Append ("--devices " + devicesHash);
            builder.Append ("--series " + settings.Series);
            builder.Append ("--locale");
            builder.AppendQuoted (settings.Locale);
            builder.Append ("--user");
            builder.AppendQuoted (userEmail);
            builder.Append ("--assembly-dir");
            builder.AppendQuoted (uitestAssemblies.MakeAbsolute (_cakeEnvironment).FullPath);

            Run (settings, builder);
        }
    }
}

