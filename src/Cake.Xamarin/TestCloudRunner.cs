using System;
using Cake.Core.IO;
using Cake.Core.Utilities;
using Cake.Core;
using System.Collections.Generic;
using Cake.Core.Tooling;

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

        /// <summary>
        /// Gets or sets the filename to output the NUnit report results.
        /// </summary>
        /// <value>The NUnit report results output file.</value>
        public FilePath NUnitXmlFile { get; set; }

        /// <summary>
        /// Gets or sets the iOS DSYM File. Optional.
        /// </summary>
        /// <value>The iOS DSYM.</value>
        public FilePath Dsym { get; set; }

        /// <summary>
        /// Gets or sets the NUnit categories to restrict running to.
        /// </summary>
        /// <value>The NUnit categories to run.</value>
        public string[] Categories { get; set; }

        /// <summary>
        /// Gets or sets the NUnit fixture to run.
        /// </summary>
        /// <value>The NUnit fixture to run.</value>
        public string Fixture { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Tests should be run in parallel by Test Method.
        /// </summary>
        /// <value><c>true</c> if Test methods should be run in parallel; otherwise, <c>false</c>.</value>
        public bool TestChunk { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Tests should be run in parallel by Test Fixture.
        /// </summary>
        /// <value><c>true</c> if Test fixtures should be run in parallel; otherwise, <c>false</c>.</value>
        public bool TestFixture { get; set; }

        /// <summary>
        /// Gets or sets the Optional App name to display in Test Cloud.
        /// </summary>
        /// <value>The optional name of the app to display in Test Cloud.</value>
        public string AppName { get; set; }

        /// <summary>
        /// Gets or sets the keystore file to explicitly use in test-cloud.
        /// </summary>
        /// <value>The keystore.</value>
        public FilePath Keystore { get; set; }

        /// <summary>
        /// Gets or sets the keystore password.
        /// </summary>
        /// <value>The keystore password.</value>
        public string KeystorePassword { get; set; }

        /// <summary>
        /// Gets or sets the keystore alias.
        /// </summary>
        /// <value>The keystore alias.</value>
        public string KeystoreAlias { get; set; }

        /// <summary>
        /// Gets or sets the keystore alias password.
        /// </summary>
        /// <value>The keystore alias password.</value>
        public string KeystoreAliasPassword { get; set; }
    }

    internal class TestCloudRunner : Cake.Core.Tooling.Tool<TestCloudSettings>
    {
        readonly ICakeEnvironment _cakeEnvironment;

        public TestCloudRunner (IFileSystem fileSystem, ICakeEnvironment cakeEnvironment, IProcessRunner processRunner, IToolLocator tools) 
            : base (fileSystem, cakeEnvironment, processRunner, tools)
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

            if (settings.Keystore != null)
            {
                builder.Append("keystore");
                builder.AppendQuoted(settings.Keystore.MakeAbsolute(_cakeEnvironment).FullPath);
                builder.AppendQuoted(settings.KeystorePassword);
                builder.AppendQuoted(settings.KeystoreAlias);
                builder.AppendQuoted(settings.KeystoreAliasPassword);
            }

            builder.Append ("--devices " + devicesHash);
            builder.Append ("--series " + settings.Series);
            builder.Append ("--locale");
            builder.AppendQuoted (settings.Locale);
            builder.Append ("--user");
            builder.AppendQuoted (userEmail);
            builder.Append ("--assembly-dir");
            builder.AppendQuoted(uitestAssemblies.MakeAbsolute(_cakeEnvironment).FullPath);

            if (settings.NUnitXmlFile != null) {
                builder.Append ("--nunit-xml");
                builder.AppendQuoted (settings.NUnitXmlFile.MakeAbsolute (_cakeEnvironment).FullPath);
            }

            if (settings.Dsym != null)
            {
                builder.Append ("--dsym");
                builder.AppendQuoted (settings.Dsym.MakeAbsolute (_cakeEnvironment).FullPath);
            }

            if (settings.Categories != null) {
                foreach (var cat in settings.Categories) {
                    builder.Append ("--category");
                    builder.AppendQuoted (cat);
                }
            }

            if (!string.IsNullOrEmpty (settings.Fixture)) {
                builder.Append ("--fixture");
                builder.AppendQuoted (settings.Fixture);
            }

            if (!string.IsNullOrEmpty (settings.AppName)) {
                builder.Append ("--app-name");
                builder.AppendQuoted (settings.AppName);
            }

            if (settings.TestChunk)
                builder.Append ("--test-chunk");

            if (settings.TestFixture)
                builder.Append ("--fixture-chunk");

            Run (settings, builder);
        }
    }
}

