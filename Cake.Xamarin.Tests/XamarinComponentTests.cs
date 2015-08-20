using System;
using Cake.Core;
using Cake.Core.IO;
using System.Collections.Generic;
using NUnit.Framework;
using Cake.Xamarin.Tests.Fakes;
using Cake.Xamarin;
using Cake.Common.Net;
using Cake.Common.IO;

namespace Cake.Xamarin.Tests
{
    [TestFixture, Category ("Xamarin Component Tests")]
    public class XamarinComponentTests
    {
        FakeCakeContext context;

        [SetUp]
        public void Setup ()
        {
            context = new FakeCakeContext ();   

            if (!context.CakeContext.FileSystem.Exist (new FilePath ("./TestProjects/tools/xamarin-component.exe"))) {
                context.CakeContext.CreateDirectory ("./TestProjects/tools/");
                context.CakeContext.DownloadFile ("https://components.xamarin.com/submit/xpkg", "./TestProjects/tools/xpkg.zip");
                context.CakeContext.Unzip ("./TestProjects/tools/xpkg.zip", "./TestProjects/tools/");
            }
        }

        [TearDown]
        public void Teardown ()
        {
            context.DumpLogs ();
        }

        [Test]
        public void RestoreComponentAndroidTest ()
        {            
            var solutionFile = new FilePath ("./TestProjects/ComponentRestoreAndroid/ComponentRestoreAndroid.sln");

            context.CakeContext.RestoreComponents (solutionFile, new XamarinComponentSettings {
                ToolPath = "./TestProjects/tools/xamarin-component.exe"
            });

            var componentLib = new FilePath ("./TestProjects/ComponentRestoreAndroid/Components/AndHUD-1.3.1/lib/android/AndHUD.dll");

            Assert.IsTrue (context.CakeContext.FileSystem.Exist (componentLib));
        }
    }
}

