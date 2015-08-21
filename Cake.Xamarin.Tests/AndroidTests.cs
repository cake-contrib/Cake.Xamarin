using NUnit.Framework;
using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using System.Collections.Generic;
using Cake.Xamarin.Tests.Fakes;
using Cake.Xamarin;
using Cake.Common.IO;

namespace Cake.Xamarin.Tests
{
    [TestFixture, Category ("AndroidTests")]
    public class AndroidTests
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
            var projectFile = context.WorkingDirectory
                .Combine ("TestProjects/HelloWorldAndroid/HelloWorldAndroid/")
                .CombineWithFilePath ("HelloWorldAndroid.csproj");

            projectFile = new FilePath ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid/HelloWorldAndroid.csproj");

            FilePath apkFile = null;

            try {
                apkFile = context.CakeContext.AndroidPackage(
                    projectFile,
                    signed,
                    c => {
                        c.Verbosity = Verbosity.Diagnostic;
                        c.Configuration = "Release";
                    });
            } catch (Exception ex) {
                Console.WriteLine(ex);
                context.DumpLogs();
                Assert.Fail(context.GetLogs());
            }
            
            Assert.IsNotNull (apkFile);
            Assert.IsNotNullOrEmpty (apkFile.FullPath);
            Assert.IsTrue (System.IO.File.Exists (apkFile.FullPath));
        }
    }
}

