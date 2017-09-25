using System;
using Cake.Core;
using Cake.Core.IO;
using System.Collections.Generic;
using Xunit;
using Cake.Xamarin;
using Cake.Common.Net;
using Cake.Common.IO;

namespace Cake.Xamarin.Tests
{
    [Trait("Category", "XamarinComponentTests")]
    public class XamarinComponentTests : Fakes.TestFixtureBase
    {
        public XamarinComponentTests ()
            : base ()
        {
            Cake.DeleteFiles ("./TestProjects/ComponentPackage/*.xam");

            if (!Cake.FileSystem.Exist (new FilePath ("./TestProjects/tools/xamarin-component.exe"))) {
                Cake.CreateDirectory ("./TestProjects/tools/");
                Cake.DownloadFile ("https://components.xamarin.com/submit/xpkg", "./TestProjects/tools/xpkg.zip");
                Cake.Unzip ("./TestProjects/tools/xpkg.zip", "./TestProjects/tools/");
            }
        }

        [Fact]
        public void RestoreComponentAndroidTest ()
        {            
            var solutionFile = new FilePath ("./TestProjects/ComponentRestoreAndroid/ComponentRestoreAndroid.sln");

            Cake.RestoreComponents (solutionFile, new XamarinComponentRestoreSettings {
                ToolPath = "./TestProjects/tools/xamarin-component.exe"
            });

            var componentLib = new FilePath ("./TestProjects/ComponentRestoreAndroid/Components/AndHUD-1.3.1/lib/android/AndHUD.dll");

            Assert.True (Cake.FileSystem.Exist (componentLib));
        }

        [Fact]
        public void PackageComponentTest ()
        {
            var yaml = new DirectoryPath ("./TestProjects/ComponentPackage/");

            Cake.PackageComponent (yaml, new XamarinComponentSettings {
                ToolPath = "./TestProjects/tools/xamarin-component.exe"
            });

            Assert.True (Cake.FileExists ("./TestProjects/ComponentPackage/testcomponent-1.0.0.0.xam"));
        }
    }
}

