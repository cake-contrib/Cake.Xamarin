using System;
using Cake.Core.IO;
using Cake.Core;
using System.Collections.Generic;

namespace Cake.Xamarin.Tests.Fakes
{
    public class FakeCakeContext
    {
        ICakeContext context;
        FakeLog log;
        DirectoryPath testsDir;

        public FakeCakeContext ()
        {
            testsDir = new DirectoryPath (
                System.IO.Path.GetFullPath(
                    System.IO.Path.Combine (AppDomain.CurrentDomain.BaseDirectory, "../../")));

            var fileSystem = new FileSystem ();
            var environment = new CakeEnvironment ();
            var globber = new Globber (fileSystem, environment);
            log = new FakeLog ();
            var args = new FakeCakeArguments ();
            var processRunner = new ProcessRunner (environment, log);
            var toolResolvers = new List<IToolResolver> ();
            var registry = new WindowsRegistry ();

            context = new CakeContext (fileSystem, environment, globber, log, args, processRunner, toolResolvers, registry);
            context.Environment.WorkingDirectory = testsDir;
        }

        public DirectoryPath WorkingDirectory {
            get { return testsDir; }
        }
            
        public ICakeContext CakeContext {
            get { return context; }
        }

        public void DumpLogs ()
        {
            foreach (var m in log.Messages)
                Console.WriteLine (m);
        }
    }
}

