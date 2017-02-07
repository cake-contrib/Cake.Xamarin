using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Xamarin
{
    /// <summary>
    /// A wrapper around the Xamarin Studio Add-in Setup Utility (<c>mdtool setup</c>).
    /// </summary>
    public class MDToolSetupRunner : Tool<MDToolSetupSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="MDToolSetupRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system</param>
        /// <param name="environment">The environment</param>
        /// <param name="processRunner">The process runner</param>
        /// <param name="tools">The tool locator</param>
        public MDToolSetupRunner(IFileSystem fileSystem, ICakeEnvironment environment, IProcessRunner processRunner, IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>Gets the name of the tool.</summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName() => "mdtool";

        /// <summary>Gets the possible names of the tool executable.</summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            yield return "mdtool";
            yield return "mdtool.exe";
            yield return "mdtool.cmd";
            yield return "mdtool.sh";
        }

        /// <summary>
        /// Creates a package from an add-in configuration file.
        /// </summary>
        /// <param name="addinFile">The addin file.</param>
        public void Pack (FilePath addinFile)
        {
            var args = GetSetupBuilder();

            args.Append("pack");
            args.Append(addinFile.MakeAbsolute(_environment).FullPath.Quote());

            Run(new MDToolSetupSettings(), args);
        }

        /// <summary>
        /// Creates a package from an add-in configuration file, output to the specified directory.
        /// </summary>
        /// <param name="addinFile">The addin file.</param>
        /// <param name="outputDirectory">The target directory for the package.</param>
        public void Pack (FilePath addinFile, DirectoryPath outputDirectory)
        {
            var args = GetSetupBuilder();

            args.Append("pack");
            args.AppendQuoted(addinFile.MakeAbsolute(_environment).FullPath);
            args.Append($"-d:{outputDirectory.FullPath.Quote()}");

            Run(new MDToolSetupSettings(), args);
        }

        /// <summary>
        /// Creates a repository index file for a directory structure.
        /// </summary>
        /// <param name="targetDirectory">Directory to scan.</param>
        public void CreateRepositoryIndex (DirectoryPath targetDirectory)
        {
            var args = GetSetupBuilder();

            args.Append("rb");
            args.AppendQuoted(targetDirectory.FullPath);

            Run(new MDToolSetupSettings(), args);
        }

        private ProcessArgumentBuilder GetSetupBuilder ()
        {
            var builder = new ProcessArgumentBuilder();
            builder.Append("setup");
            return builder;
        }
    }
}
