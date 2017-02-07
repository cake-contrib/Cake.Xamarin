using System;
using Cake.Core;
using Cake.Core.IO;
using System.Collections.Generic;
using NUnit.Framework;
using Cake.Xamarin;
using Cake.Common.Net;
using Cake.Common.IO;

namespace Cake.Xamarin.Tests
{
    [TestFixture, Category ("XamarinComponentTests")]
    public class XamarinComponentTests : Fakes.TestFixtureBase
    {
        [SetUp]
        public override void Setup ()
        {
			base.Setup();

            Cake.DeleteFiles ("./TestProjects/ComponentPackage/*.xam");

            if (!Cake.FileSystem.Exist (new FilePath ("./TestProjects/tools/xamarin-component.exe"))) {
                Cake.CreateDirectory ("./TestProjects/tools/");
                Cake.DownloadFile ("https://components.xamarin.com/submit/xpkg", "./TestProjects/tools/xpkg.zip");
                Cake.Unzip ("./TestProjects/tools/xpkg.zip", "./TestProjects/tools/");
            }
        }

        [Test]
        public void RestoreComponentAndroidTest ()
        {            
            var solutionFile = new FilePath ("./TestProjects/ComponentRestoreAndroid/ComponentRestoreAndroid.sln");

            Cake.RestoreComponents (solutionFile, new XamarinComponentRestoreSettings {
                ToolPath = "./TestProjects/tools/xamarin-component.exe"
            });

            var componentLib = new FilePath ("./TestProjects/ComponentRestoreAndroid/Components/AndHUD-1.3.1/lib/android/AndHUD.dll");

            Assert.IsTrue (Cake.FileSystem.Exist (componentLib));
        }

        [Test]
        public void PackageComponentTest ()
        {
            var yaml = new DirectoryPath ("./TestProjects/ComponentPackage/");

            Cake.PackageComponent (yaml, new XamarinComponentSettings {
                ToolPath = "./TestProjects/tools/xamarin-component.exe"
            });

            Assert.IsTrue (Cake.FileExists ("./TestProjects/ComponentPackage/testcomponent-1.0.0.0.xam"));
        }
    }
}

