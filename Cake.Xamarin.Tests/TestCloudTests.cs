using System;
using NUnit.Framework;
using Cake.Common.IO;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools;
using System.Linq;
using Cake.Common.Text;

namespace Cake.Xamarin.Tests
{
    [TestFixture, Category ("TestCloudTests")]
    public class TestCloudTests : Fakes.TestFixtureBase
    {
        [Test]
        public void UITestsTest ()
        {
            Cake.NuGetRestore ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid.sln");

            // Try and find a console runner from the installed nugets
            var nunitConsolePath = Cake.GetFiles ("./TestProjects/HelloWorldAndroid/**/nunit-console.exe")
                .FirstOrDefault ();

            // Build the sln so the unit tests assembly gets built
            Cake.DotNetBuild ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid.sln");

            // Build the .apk to test
            var apk = Cake.AndroidPackage ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid/HelloWorldAndroid.csproj");

            // Copy to the expected location of the UITests
            Cake.CopyFile (apk, "./TestProjects/HelloWorldAndroid/HelloWorldAndroid.UITests/bin/Debug/app.apk");

            // Run the uitests
            Cake.UITest ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid.UITests/bin/Debug/HelloWorldAndroid.UITests.dll", 
                new Cake.Common.Tools.NUnit.NUnitSettings {
                    ToolPath = nunitConsolePath
                });
        }

        [Test]
        public void TestCloudTest ()
        {
            Cake.NuGetRestore ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid.sln");

            // Try and find a test-cloud-exe from the installed nugets
            var testCloudExePath = Cake.GetFiles ("./TestProjects/HelloWorldAndroid/**/test-cloud.exe")
                .FirstOrDefault ();

            // Build the sln so the unit tests assembly gets built
            Cake.DotNetBuild ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid.sln");

            // Build the .apk to test
            var apk = Cake.AndroidPackage ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid/HelloWorldAndroid.csproj", true);

            var xtcApiKey = Cake.TransformTextFile ("../xtc-api-key").ToString ();
            var xtcEmail = Cake.TransformTextFile ("../xtc-email").ToString ();

            // Run testcloud
            Cake.TestCloud (apk,
                xtcApiKey,
                "2b9b256d",
                xtcEmail,
                "./TestProjects/HelloWorldAndroid/HelloWorldAndroid.UITests/bin/Debug/",
                new TestCloudSettings {
                    ToolPath = testCloudExePath
                });

            Cake.UITest ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid.UITests/bin/Debug/HelloWorldAndroid.UITests.dll", 
                new Cake.Common.Tools.NUnit.NUnitSettings {
                    ToolPath = testCloudExePath
                });
        }
    }
}

