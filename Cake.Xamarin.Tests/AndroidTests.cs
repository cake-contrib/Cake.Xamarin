using NUnit.Framework;
using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using System.Collections.Generic;
using Cake.Xamarin.Fakes;
using Cake.Xamarin;
using Cake.Common.IO;

namespace Cake.Xamarin.Tests
{
    [TestFixture, Category ("AndroidTests")]
    public class AndroidTests : TestFixtureBase
    {
        [Test]
        public void AndroidPackageSignedTest ()
        {
            androidPackageTest (true);
        }

        [Test]
        public void AndroidPackageUnsignedTest ()
        {
            androidPackageTest (false);
        }

        void androidPackageTest (bool signed)
        {                        
            var projectFile = base.WorkingDirectory
                .Combine ("TestProjects/HelloWorldAndroid/HelloWorldAndroid/")
                .CombineWithFilePath ("HelloWorldAndroid.csproj");

            projectFile = new FilePath ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid/HelloWorldAndroid.csproj");

            FilePath apkFile = null;

            apkFile = Cake.AndroidPackage(
                projectFile,
                signed,
                c => {
                    c.Verbosity = Verbosity.Diagnostic;
                    c.Configuration = "Release";
                });

            Assert.IsNotNull (apkFile);
            Assert.IsNotNull (apkFile.FullPath);
            Assert.IsNotEmpty (apkFile.FullPath);
            Assert.IsTrue (System.IO.File.Exists (apkFile.FullPath));
        }
    }
}

