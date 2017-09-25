using Cake.Core;
using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Common.IO;

namespace Cake.Xamarin.Fakes
{
	public abstract class TestFixtureBase : IDisposable
	{
		FakeCakeContext context;

		public ICakeContext Cake { get { return context.CakeContext; } }

		public DirectoryPath WorkingDirectory { get { return context.WorkingDirectory; } }

		public TestFixtureBase()
		{
			context = new FakeCakeContext();
			context.CakeContext.Environment.WorkingDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "..", "..");

			context.CakeContext.CleanDirectories("./TestProjects/**/bin");
			context.CakeContext.CleanDirectories("./TestProjects/**/obj");
		}

		public void Dispose()
		{
			context.DumpLogs();
		}
	}
}