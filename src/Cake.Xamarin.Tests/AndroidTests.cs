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

            Assert.NotNull (apkFile);
            Assert.NotNull (apkFile.FullPath);
            Assert.NotEmpty (apkFile.FullPath);
            Assert.True (System.IO.File.Exists (apkFile.FullPath));
        }
    }
}

