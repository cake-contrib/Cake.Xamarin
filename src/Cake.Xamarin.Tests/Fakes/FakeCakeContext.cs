using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Testing;
using NSubstitute;

namespace Cake.Xamarin.Fakes
{
	public class FakeCakeContext
	{
		ICakeContext context;
		FakeCakeLog log;
		DirectoryPath testsDir;

		public FakeCakeContext()
		{
			testsDir = new DirectoryPath(System.IO.Path.GetFullPath(AppContext.BaseDirectory));

			log = new FakeCakeLog();
			var fileSystem = new FileSystem();
			var environment = new CakeEnvironment(new CakePlatform(), new CakeRuntime(), log);
			var globber = new Globber(fileSystem, environment);
			var args = new FakeCakeArguments();
			var registry = new WindowsRegistry();
			var toolRepo = new ToolRepository(environment);
			var config = new FakeConfiguration();
			var toolResolutionStrategy = new ToolResolutionStrategy(fileSystem, environment, globber, config);
			var toolLocator = new ToolLocator(environment, toolRepo, toolResolutionStrategy);
			var processRunner = new ProcessRunner(fileSystem, environment, log, toolLocator, config);
			var data = Substitute.For<ICakeDataService>();
			context = new CakeContext(fileSystem, environment, globber, log, args, processRunner, registry, toolLocator, data, config);
			context.Environment.WorkingDirectory = testsDir;
		}

		public DirectoryPath WorkingDirectory
		{
			get { return testsDir; }
		}

		public ICakeContext CakeContext
		{
			get { return context; }
		}

		public string GetLogs()
		{
			return string.Join(Environment.NewLine, log.Messages);
		}

		public void DumpLogs()
		{
			foreach (var m in log.Messages)
				Console.WriteLine(m);
		}
	}
}