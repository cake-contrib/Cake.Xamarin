using System;
using NUnit.Framework;
using Cake.Xamarin.Tests.Fakes;
using Cake.Core.IO;
using Cake.Common.IO;

namespace Cake.Xamarin.Tests
{
    [TestFixture, Category ("iOSTests")]
    public class iOSTests
    {
        FakeCakeContext context;

        [SetUp]
        public void Setup ()
        {
            context = new FakeCakeContext ();

            context.CakeContext.CleanDirectories ("./TestProjects/**/bin");
            context.CakeContext.CleanDirectories ("./TestProjects/**/obj");
        }

        [TearDown]
        public void Teardown ()
        {
            context.DumpLogs ();
        }
    }

    [TestFixture, Category ("iOSTestsMacOnly")]
    public class iOSTestsMacOnly
    {
        FakeCakeContext context;

        [SetUp]
        public void Setup ()
        {
            context = new FakeCakeContext ();

            context.CakeContext.CleanDirectories ("./TestProjects/**/bin");
            context.CakeContext.CleanDirectories ("./TestProjects/**/obj");
        }

        [TearDown]
        public void Teardown ()
        {
            context.DumpLogs ();
        }

        [Test]
        public void MDToolBuildTest ()
        {
            context.CakeContext.MDToolBuild ("./TestProjects/HelloWorldiOS/HelloWorldiOS.sln");

            Assert.IsTrue (context.CakeContext.FileSystem.Exist (new FilePath ("./TestProjects/HelloWorldiOS/HelloWorldiOS/bin/iPhoneSimulator/Debug/HelloWorldiOS.exe")));
        }

        [Test]
        public void MDToolArchiveTest ()
        {
            context.CakeContext.MDToolArchive ("./TestProjects/HelloWorldiOS/HelloWorldiOS.sln", "HelloWorldiOS", s => {
                s.Configuration = "Release|iPhone";
            });
            Assert.IsTrue (context.CakeContext.FileSystem.Exist (new DirectoryPath ("./TestProjects/HelloWorldiOS/HelloWorldiOS/bin/iPhone/Release/HelloWorldiOS.app.dSYM")));
        }
    }
}
