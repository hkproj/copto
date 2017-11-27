#addin "Cake.Incubator"
//#tool "nuget:?package=NUnit.ConsoleRunner"

var target = Argument("target", "Default");
var configurations = new []{ "Release" };
var frameworks = new []{ "netstandard2.0" };
var nugetOutputDirectory = "./artifacts/";
var projects = new [] { "Copto", "Copto.Tests" };
var pack_projects = new [] { "Copto" };

string GetProjectDirectory(string project) => "./src/" + project + "/";
string GetOutputDirectory(string project, string framework, string configuration) => GetProjectDirectory(project) + "bin/" + configuration + "/" + framework + "/";
string GetProjectFile(string project) => GetProjectDirectory(project) + project + ".csproj";


Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() => {
        foreach(var project in projects) {
            foreach(var framework in frameworks) {
                foreach(var configuration in configurations) {
                    DotNetCoreBuild(GetProjectFile(project), new DotNetCoreBuildSettings() {
                        Configuration = configuration,
                        Framework = framework
                    });
                }
            }
        }
    });

Task("Clean")
    .Does(() => {

        // Delete artifacts folder
        if (DirectoryExists(nugetOutputDirectory)) {
            DeleteDirectory(nugetOutputDirectory, new DeleteDirectorySettings() {
                Recursive = true,
                Force = true
            });
        }

        // For each project output folder, delete its content
        foreach(var project in projects) {
            foreach(var framework in frameworks) {
                foreach(var configuration in configurations) {
                    var conf_dir = GetOutputDirectory(project, framework, configuration);
                    Verbose("Deleting directory: " + conf_dir);

                    if (DirectoryExists(conf_dir)) {
                        DeleteDirectory(conf_dir, new DeleteDirectorySettings() {
                            Recursive = true,
                            Force = true
                        });
                    }

                    CreateDirectory(conf_dir);
                }
            }
        }
    });

Task("Restore")
    .Does(() => {
        DotNetCoreRestore("src");
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
		Warning("TODO: SKIPPED");
    });

Task("Package")
    .IsDependentOn("Test")
    .Does(() => {
        foreach(var project in pack_projects) {
            foreach( var configuration in configurations) {
                DotNetCorePack(GetProjectFile(project), new DotNetCorePackSettings() {
                    OutputDirectory = nugetOutputDirectory,
                    Configuration = configuration
                });
            }
        }
    });

Task("Default")
    .IsDependentOn("Package");

RunTarget("Default");