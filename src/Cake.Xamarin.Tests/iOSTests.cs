using System;
using Xunit;
using Cake.Core.IO;
using Cake.Common.IO;
using Cake.Core.Diagnostics;

namespace Cake.Xamarin.Tests
{
    [Trait ("Category", "iOSTests")]
    public class iOSTests : Fakes.TestFixtureBase
    {
        //[Fact]
        public void IpaTests()
        {
            var projectFile = new FilePath("./TestProjects/HelloWorldiOS/HelloWorldiOS/HelloWorldiOS.csproj");

            var ipaFile = Cake.BuildiOSIpa(
                projectFile,
                "Release",
                "iPhone",
                c => {
                    c.Verbosity = Verbosity.Diagnostic;
                });

            Assert.NotNull(ipaFile);
            Assert.NotNull(ipaFile.FullPath);
            Assert.NotEmpty(ipaFile.FullPath);
            Assert.True(System.IO.File.Exists(ipaFile.FullPath));
        }
    }
}
