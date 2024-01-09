using System.Security.AccessControl;
using System.Text.Json.Nodes;
using Microsoft.Win32;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: Digi21.Digi3D.App.exe <.runtimeconfig.json>");
    return 1;
}

if (!File.Exists(args[0]))
{
    return 0;
}

var localMachine = Registry.LocalMachine;

var software = localMachine.OpenSubKey("SOFTWARE", RegistryRights.ReadKey);
if( software is null)
    return 2;

var digi21 = software.OpenSubKey("Digi21", RegistryRights.ReadKey);
if (digi21 is null)
{
    Console.Error.WriteLine("Digi21 not found in registry");
    return 3;
}

var digi3D = digi21.OpenSubKey("Digi3D.NET", RegistryRights.ReadKey);
if (digi3D is null)
{
    Console.Error.WriteLine("Digi3D.NET not found in registry");
    return 4;
}

var app = digi3D.OpenSubKey("App", RegistryRights.ReadKey);
if (app is null)
{
    Console.Error.WriteLine("App not found in registry");
    return 5;
}

var configuration = app.OpenSubKey("Configuration", RegistryRights.ReadKey);
if (configuration is null)
{
    Console.Error.WriteLine("Configuration not found in registry");
    return 6;
}

if (configuration.GetValue("InstallDir") is not string directorioInstalacion)
{
    Console.Error.WriteLine("InstallDir not found in registry");
    return 7;
}

var runtimeconfigjson = JsonNode.Parse(File.ReadAllText(args[0]));
if (runtimeconfigjson is null)
{
    Console.Error.WriteLine($"Failed to parse {args[0]}");
    return 8;
}

var runtimeOptions = runtimeconfigjson["runtimeOptions"];
if (runtimeOptions is null)
{
    Console.Error.WriteLine("Failed to find 'runtimeOptions'");
    return 9;
}

var additionalProbingPaths = runtimeOptions["additionalProbingPaths"];
if (additionalProbingPaths is null)
{
    runtimeOptions["additionalProbingPaths"] = JsonNode.Parse($"""
                                                               [
                                                                 "{directorioInstalacion.Replace("\\", @"\\")}"
                                                               ]
                                                               """);
}
else
{
    if (additionalProbingPaths.AsArray().Any(additionalProbingPath => additionalProbingPath != null && additionalProbingPath.ToString() == directorioInstalacion))
        return 0;

    additionalProbingPaths.AsArray().Add(directorioInstalacion);
}
File.WriteAllText(args[0], runtimeconfigjson.ToString());

return 0;