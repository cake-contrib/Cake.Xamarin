# Cake.Xamarin
A set of aliases for http://cakebuild.net to help with Xamarin projects.

![AppVeyor](https://ci.appveyor.com/api/projects/status/github/redth/Cake.Xamarin)

The following Aliases are available:

### AndroidPackage
Creates an android .APK package file
```csharp
FilePath AndroidPackage (FilePath projectFile, bool sign = false, Action<DotNetBuildSettings> configurator = null)
```

### iOSArchive
Creates an archive of the Xamarin.iOS app
```csharp
void iOSArchive (FilePath solutionFile, string projectName)
```

```csharp
void iOSArchive (FilePath solutionFile, string projectName, MDToolSettings settings)
```
       
        
        
### iOSBuild
Builds a Xamarin.iOS project using MDTool (Mac Only)
```csharp
void iOSBuild (FilePath projectFile)
```

```csharp
void iOSBuild (FilePath projectFile, MDToolSettings settings)
```

### iOSMSBuild
Builds a Xamarin.iOS using MSBuild on Windows or XBuild on Mono
```csharp
void iOSMSBuild (FilePath projectOrSolutionFile)
```

```csharp
iOSMSBuild (FilePath projectOrSolutionFile, Action<DotNetBuildSettings> configurator)
```

### RestoreComponents
Restores Xamarin Components for a given project
```csharp
void RestoreComponents (FilePath projectFile)
```

```csharp
void RestoreComponents (FilePath projectFile, XamarinComponentSettings settings)
```

### UITest
Runs UITests for a given assembly containing UITests
```csharp
void UITest (FilePath testsAssembly, NUnitSettings nunitSettings = null)
```

### TestCloud
Uploads a given .APK and directory of UITest assemblies to Test Cloud and runs the tests
```csharp
void TestCloud (FilePath apkFile, string apiKey, string devicesHash, string userEmail, DirectoryPath uitestsAssemblies)
```

```csharp
void TestCloud (FilePath apkFile, string apiKey, string devicesHash, string userEmail, DirectoryPath uitestsAssemblies, TestCloudSettings settings)
```

## Apache License 2.0
Apache Cake.Xamarin Copyright 2015. The Apache Software Foundation This product includes software developed at The Apache Software Foundation (http://www.apache.org/).
