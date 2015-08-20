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

        [Test]
        public void iOSMSBuildTest ()
        {
            context.CakeContext.iOSMSBuild ("./TestProjects/HelloWorldiOS/HelloWorldiOS.sln");

            Assert.IsTrue (context.CakeContext.FileSystem.Exist (new FilePath ("./TestProjects/HelloWorldiOS/HelloWorldiOS/bin/iPhoneSimulator/Debug/HelloWorldiOS.exe")));
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
        public void iOSBuildTest ()
        {
            context.CakeContext.iOSBuild ("./TestProjects/HelloWorldiOS/HelloWorldiOS.sln");

            Assert.IsTrue (context.CakeContext.FileSystem.Exist (new FilePath ("./TestProjects/HelloWorldiOS/HelloWorldiOS/bin/iPhoneSimulator/Debug/HelloWorldiOS.exe")));
        }

        [Test]
        public void iOSArchiveTest ()
        {
            context.CakeContext.iOSArchive ("./TestProjects/HelloWorldiOS/HelloWorldiOS.sln", "HelloWorldiOS");
            Assert.IsTrue (context.CakeContext.FileSystem.Exist (new DirectoryPath ("./TestProjects/HelloWorldiOS/HelloWorldiOS/bin/iPhone/Release/HelloWorldiOS.app.dSYM")));
        }
    }
}

