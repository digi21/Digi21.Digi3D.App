# Digi21.Digi3D.App

If a user application uses any of the NuGet packages provided by the [Digi3D.NET](https://www.digi21.net/Digi3D) application, such as [Digi21.DigiNG.IO.Bin](https://www.nuget.org/packages/Digi21.DigiNG.Io.Bin), as in _.NET8.0_ there is no concept of _Global Area Cache_, when executing the application it will not locate the assemblies provided by _Digi3D.NET_.

* A solution would be to copy the user application in the path where _Digi3D.NET_ is installed, but this would require administrator permissions.
* Another solution consists of adding in the _.runtimeconfig.json_ file of the user application an `additionalProbingPaths` node with the path to the _Digi3D.NET_ installation directory, this way the user application can be in any directory and it will locate correctly the assemblies provided by _Digi3D.NET_.

This package adds a _PostBuildEvent_ (which is executed each time the dependent application is recompiled) that executes a tool that adds the installation path of the Digi3D.NET program in the `additionalProbingPaths` node of the _.runtimeconfig.json_ file generated when compiling.
