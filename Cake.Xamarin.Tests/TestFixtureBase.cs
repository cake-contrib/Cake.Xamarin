using Cake.Core;
using Cake.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Cake.Common.IO;

namespace Cake.Xamarin.Fakes
{
	[TestFixture]
	public abstract class TestFixtureBase
	{
		FakeCakeContext context;

		public ICakeContext Cake { get { return context.CakeContext; } }

		public DirectoryPath WorkingDirectory { get { return context.WorkingDirectory; } }

		[OneTimeSetUp]
		public void RunBeforeAnyTests()
		{
			Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(typeof(TestFixtureBase).Assembly.Location);
		}

		[SetUp]
		public virtual void Setup()
		{
			context = new FakeCakeContext();
			context.CakeContext.Environment.WorkingDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "..", "..");

			context.CakeContext.CleanDirectories("./TestProjects/**/bin");
			context.CakeContext.CleanDirectories("./TestProjects/**/obj");
		}

		[TearDown]
		public void Teardown()
		{
			context.DumpLogs();
		}
	}
}