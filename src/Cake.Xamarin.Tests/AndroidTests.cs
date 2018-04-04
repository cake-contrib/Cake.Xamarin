using Xunit;
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
    [Trait ("Category", "AndroidTests")]
    public class AndroidTests : TestFixtureBase
    {
        //[Fact]
        public void AndroidPackageSignedTest ()
        {
            androidPackageTest (true);
        }

        //[Fact]
        public void AndroidPackageUnsignedTest ()
        {
            androidPackageTest (false);
        }

        void androidPackageTest (bool signed)
        {                        
            var projectFile = new FilePath ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid/HelloWorldAndroid.csproj");

            var apkFile = Cake.BuildAndroidApk(
                projectFile,
                signed,
                "Release",
                c => {
                    c.Verbosity = Verbosity.Diagnostic;
                });

            Assert.NotNull (apkFile);
            Assert.NotNull (apkFile.FullPath);
            Assert.NotEmpty (apkFile.FullPath);
            Assert.True (System.IO.File.Exists (apkFile.FullPath));
        }
    }
}

