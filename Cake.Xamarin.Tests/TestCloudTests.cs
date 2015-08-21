using System;
using Cake.Xamarin.Tests.Fakes;
using NUnit.Framework;
using Cake.Common.IO;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools;
using System.Linq;
using Cake.Common.Text;

namespace Cake.Xamarin.Tests
{
    [TestFixture, Category ("TestCloudTests")]
    public class TestCloudTests
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
        public void UITestsTest ()
        {
            context.CakeContext.NuGetRestore ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid.sln");

            // Try and find a console runner from the installed nugets
            var nunitConsolePath = context.CakeContext.GetFiles ("./TestProjects/HelloWorldAndroid/**/nunit-console.exe")
                .FirstOrDefault ();

            // Build the sln so the unit tests assembly gets built
            context.CakeContext.DotNetBuild ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid.sln");

            // Build the .apk to test
            var apk = context.CakeContext.AndroidPackage ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid/HelloWorldAndroid.csproj");

            // Copy to the expected location of the UITests
            context.CakeContext.CopyFile (apk, "./TestProjects/HelloWorldAndroid/HelloWorldAndroid.UITests/bin/Debug/app.apk");

            try {
                // Run the uitests
                context.CakeContext.UITest ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid.UITests/bin/Debug/HelloWorldAndroid.UITests.dll", 
                    new Cake.Common.Tools.NUnit.NUnitSettings {
                        ToolPath = nunitConsolePath
                    });
            } catch (Exception ex) {
                Console.WriteLine (ex);
                Console.WriteLine (context.GetLogs ());
                Assert.Fail (context.GetLogs ());
            }
        }

        [Test]
        public void TestCloudTest ()
        {
            context.CakeContext.NuGetRestore ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid.sln");

            // Try and find a test-cloud-exe from the installed nugets
            var testCloudExePath = context.CakeContext.GetFiles ("./TestProjects/HelloWorldAndroid/**/test-cloud.exe")
                .FirstOrDefault ();

            // Build the sln so the unit tests assembly gets built
            context.CakeContext.DotNetBuild ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid.sln");

            // Build the .apk to test
            var apk = context.CakeContext.AndroidPackage ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid/HelloWorldAndroid.csproj", true);

            var xtcApiKey = context.CakeContext.TransformTextFile ("../xtc-api-key").ToString ();
            var xtcEmail = context.CakeContext.TransformTextFile ("../xtc-email").ToString ();

            try {
                // Run testcloud
                context.CakeContext.TestCloud (apk,
                    xtcApiKey,
                    "2b9b256d",
                    xtcEmail,
                    "./TestProjects/HelloWorldAndroid/HelloWorldAndroid.UITests/bin/Debug/",
                    new TestCloudSettings {
                        ToolPath = testCloudExePath
                    });

                context.CakeContext.UITest ("./TestProjects/HelloWorldAndroid/HelloWorldAndroid.UITests/bin/Debug/HelloWorldAndroid.UITests.dll", 
                    new Cake.Common.Tools.NUnit.NUnitSettings {
                        ToolPath = testCloudExePath
                    });
            } catch (Exception ex) {
                Console.WriteLine (ex);
                Console.WriteLine (context.GetLogs ());
                Assert.Fail (context.GetLogs ());
            }
        }
    }
}

