using System;
using NUnit.Framework;
using Cake.Core.IO;
using Cake.Common.IO;

namespace Cake.Xamarin.Tests
{
    [TestFixture, Category ("iOSTests")]
    public class iOSTests : Fakes.TestFixtureBase
    {
       
    }

    [TestFixture, Category ("iOSTestsMacOnly")]
    public class iOSTestsMacOnly : Fakes.TestFixtureBase
    {
        [Test]
        public void MDToolBuildTest ()
        {
            Cake.MDToolBuild ("./TestProjects/HelloWorldiOS/HelloWorldiOS.sln");

            Assert.IsTrue (Cake.FileSystem.Exist (new FilePath ("./TestProjects/HelloWorldiOS/HelloWorldiOS/bin/iPhoneSimulator/Debug/HelloWorldiOS.exe")));
        }

        [Test]
        public void MDToolArchiveTest ()
        {
            Cake.MDToolArchive ("./TestProjects/HelloWorldiOS/HelloWorldiOS.sln", "HelloWorldiOS", s => {
                s.Configuration = "Release|iPhone";
            });
            Assert.IsTrue (Cake.FileSystem.Exist (new DirectoryPath ("./TestProjects/HelloWorldiOS/HelloWorldiOS/bin/iPhone/Release/HelloWorldiOS.app.dSYM")));
        }
    }
}
