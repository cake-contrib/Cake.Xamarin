# Cake.Xamarin
A set of aliases for http://cakebuild.net to help with Xamarin projects.

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
Builds a Xamarin.iOS project
```csharp
void iOSBuild (FilePath projectFile)
```

```csharp
void iOSBuild (FilePath projectFile, MDToolSettings settings)
```

### RestoreComponents
Restores Xamarin Components for a given project
```csharp
void RestoreComponents (FilePath projectFile)
```

```csharp
void RestoreComponents (FilePath projectFile, XamarinComponentSettings settings)
```




## Apache License 2.0
Apache Cake.Xamarin Copyright 2015. The Apache Software Foundation This product includes software developed at The Apache Software Foundation (http://www.apache.org/).
