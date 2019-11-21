// CAKE FILE

// powershell -File build.ps1 -ScriptArgs '-runtime="osx.10.12-x64"'
// ./build.sh --runtime="osx.10.12-x64"
// or: ./build.sh -Target="CI"

// Windows 32 bits: 'win7-x86'
// Mac: 'osx.10.12-x64'
// Raspberry Pi: 'linux-arm'
// ARM64: 'linux-arm64'


// For the step CoverageReport
#tool "nuget:?package=ReportGenerator"

// SonarQube
#tool nuget:?package=MSBuild.SonarQube.Runner.Tool
#addin nuget:?package=Cake.Sonar

// Get Git info
#addin nuget:?package=Cake.Git

// For NPM
#addin "Cake.Npm"


// Target - The task you want to start. Runs the Default task if not specified.
var target = Argument("Target", "Default");
var configuration = Argument("Configuration", "Release");

var genericName = "generic-netcore";
var runtime = Argument("runtime", genericName);


Information($"Running target {target} in configuration {configuration}");
Information($"\n>> Try to build on {runtime}");

if(runtime == null || runtime == "" ) runtime = genericName;
var distDirectory = Directory($"./{runtime}");
var genericDistDirectory = Directory($"./{genericName}");

var projectNames = new List<string>{
    "tabletop"
};


var testProjectNames = new List<string>{
    "tabletop.test"
};

// Deletes the contents of the Artifacts folder if it contains anything from a previous build.
Task("Clean")
    .Does(() =>
    {

        if (FileExists($"tabletop-{genericDistDirectory}.zip"))
        {
            DeleteFile($"tabletop-{genericDistDirectory}.zip");
        }

        if (FileExists($"tabletop-{distDirectory}.zip"))
        {
            DeleteFile($"tabletop-{distDirectory}.zip");
        }

        CleanDirectory(distDirectory);
        CleanDirectory(genericDistDirectory);
    });

// Run dotnet restore to restore all package references.
Task("Restore")
    .Does(() =>
    {
        Environment.SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT","true");

        // make a new list
        var restoreProjectNames = new List<string>(projectNames);
        restoreProjectNames.AddRange(testProjectNames);

        // now restore test with generic settings (always)
        // used to get all dependencies
        DotNetCoreRestore(".",
            new DotNetCoreRestoreSettings());

        if(runtime == genericName) return;

        System.Console.WriteLine($"> restore for {runtime}");

        var dotnetRestoreSettings = new DotNetCoreRestoreSettings{
            Runtime = runtime
        };

        foreach(var projectName in projectNames)
        {
            System.Console.WriteLine($"./{projectName}/{projectName}.csproj");
            DotNetCoreRestore($"./{projectName}/{projectName}.csproj",
                dotnetRestoreSettings);
        }


    });

// Build using the build configuration specified as an argument.
 Task("Build")
    .Does(() =>
    {
        var dotnetBuildSettings = new DotNetCoreBuildSettings()
        {
            Configuration = configuration,
            ArgumentCustomization = args => args.Append("--no-restore"),
        };

        System.Console.WriteLine($"> build: {runtime}");
        // generic build for mstest
        DotNetCoreBuild(".",
            dotnetBuildSettings);

        // rebuild for specific target
        if(runtime != genericName) {

            System.Console.WriteLine($"> rebuild for specific target {runtime}");
            dotnetBuildSettings.Runtime = runtime;

            foreach(var projectName in projectNames)
            {
                System.Console.WriteLine($"./{projectName}/{projectName}.csproj");
                DotNetCoreBuild($"./{projectName}/{projectName}.csproj",
                    dotnetBuildSettings);
            }

        }


    });

// Look under a 'Tests' folder and run dotnet test against all of those projects.
// Then drop the XML test results file in the Artifacts folder at the root.
Task("TestNetCore")
    .Does(() =>
    {
        var projects = GetFiles("./*test/*.csproj");
        foreach(var project in projects)
        {
            Information("Testing project " + project);

            DotNetCoreTest(
                project.ToString(),
                new DotNetCoreTestSettings()
                {
                    Configuration = configuration,
                    NoBuild = true,
                    ArgumentCustomization = args => args.Append("--no-restore")
                                             .Append("/p:CollectCoverage=true")
                                             .Append("/p:CoverletOutputFormat=cobertura")
                                             .Append("/p:ThresholdType=line")
                                             .Append("/p:hideMigrations=\"true\"")
                                             .Append("/p:Exclude=\"[tabletop.Views]*\"")
                                             .Append("/p:ExcludeByFile=\"**/Migrations/*\"") // (, comma seperated)
                                             .Append("/p:CoverletOutput=\"netcore-coverage.cobertura.xml\"")
                });

            // Check if there is any output
            string parent = System.IO.Directory.GetParent(project.ToString()).FullName;
            string coverageFile = System.IO.Path.Combine(parent, "netcore-coverage.cobertura.xml");

            Information("CoverageFile " + coverageFile);

            if (!FileExists(coverageFile)) {
                throw new Exception("CoverageFile missing " + coverageFile);
            }
        }
    });



Task("CoverageReport")
    .Does(() =>
    {
        var projects = GetFiles("./*test/netcore-coverage.cobertura.xml");
        foreach(var project in projects)
        {
            Information("CoverageReport project " + project);
            // Generate html files for reports
            var reportFolder = project.ToString().Replace("netcore-coverage.cobertura.xml","coverage-report");
            ReportGenerator(project, reportFolder, new ReportGeneratorSettings{
                ReportTypes = new[] { ReportGeneratorReportType.HtmlInline }
            });
            // Zip entire folder
            Zip(reportFolder, $"{reportFolder}.zip");
        }
    });

// Publish the app to the /dist folder
Task("PublishWeb")
    .Does(() =>
    {
        foreach (var projectName in projectNames)
        {
            System.Console.WriteLine($"./{projectName}/{projectName}.csproj");

            var dotnetPublishSettings = new DotNetCorePublishSettings()
            {
                Configuration = configuration,
                OutputDirectory = genericDistDirectory, // <= first to generic
                ArgumentCustomization = args => args.Append("--no-restore"),
            };

            // The items are already build {generic build}
            DotNetCorePublish(
                $"./{projectName}/{projectName}.csproj",
                dotnetPublishSettings
            );

            // also publish the other files for runtimes
            if(runtime == genericName) continue;

            dotnetPublishSettings.Runtime = runtime;
            dotnetPublishSettings.OutputDirectory = distDirectory; // <= then to linux-arm

            DotNetCorePublish(
                $"./{projectName}/{projectName}.csproj",
                dotnetPublishSettings
            );

        }

    });

Task("Zip")
    .Does(() =>
    {
        // for generic projects
        System.Console.WriteLine($"./{genericDistDirectory}", $"tabletop-{genericDistDirectory}.zip");
        Zip($"./{genericDistDirectory}", $"tabletop-{genericDistDirectory}.zip");

        if(runtime == genericName) return;
        // for runtime projects e.g. linux-arm or osx.10.12-x64

        System.Console.WriteLine($"./{distDirectory}", $"tabletop-{distDirectory}.zip");
        Zip($"./{distDirectory}", $"tabletop-{distDirectory}.zip");

    });

Task("SonarBegin")
   .Does(() => {
        var key = EnvironmentVariable("TABLETOP_SONAR_KEY");
        var login = EnvironmentVariable("TABLETOP_SONAR_LOGIN");
        var organisation = EnvironmentVariable("TABLETOP_SONAR_ORGANISATION");

        var url = EnvironmentVariable("TABLETOP_SONAR_URL");
        if(string.IsNullOrEmpty(url)) {
            url = "https://sonarcloud.io";
        }

        if( string.IsNullOrEmpty(key) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(organisation) ) {
            Information($">> SonarQube is disabled $ key={key}|login={login}|organisation={organisation}");
            return;
        }

        // get first test project
        var firstTestProject = GetDirectories("./*test").FirstOrDefault().ToString();
        string netCoreCoverageFile = System.IO.Path.Combine(firstTestProject, "netcore-coverage.opencover.xml");


        // Current branch name
        string parent = System.IO.Directory.GetParent(".").FullName;
        var gitBranch = GitBranchCurrent(parent);
        var branchName = gitBranch.FriendlyName;
        if(branchName == "(no branch)") branchName = "master";

        /* branchName = "master"; */

        SonarBegin(new SonarBeginSettings{
            Name = "Tabletop",
            Key = key,
            Login = login,
            Verbose = false,
            Url = url,
            Branch = branchName,
            UseCoreClr = true,
            OpenCoverReportsPath = netCoreCoverageFile,
            ArgumentCustomization = args => args
                .Append($"/o:" + organisation)
                .Append($"/d:sonar.coverage.exclusions=\"**/setupTests.js,**/react-app-env.d.ts,**/service-worker.ts,*webhtmlcli/**/*.js,**/wwwroot/js/**/*,**/tabletop/Migrations/*,**/*spec.ts,**/*spec.tsx,**/src/index.tsx\"")
                .Append($"/d:sonar.exclusions=\"**/setupTests.js,**/react-app-env.d.ts,**/service-worker.ts,*webhtmlcli/**/*.js,**/wwwroot/js/**/*,**/tabletop/Migrations/*,**/*spec.tsx,**/*spec.ts,**/src/index.tsx\"")
        });
  });

Task("SonarEnd")
  .Does(() => {
    var login = EnvironmentVariable("TABLETOP_SONAR_LOGIN");
    if( string.IsNullOrEmpty(login) ) {
        Information($">> SonarQube is disabled $ login={login}");
        return;
    }
    SonarEnd(new SonarEndSettings {
        Login = login,
        Silent = true,
    });
  });

// A meta-task that runs all the steps to Build and Test the app
Task("BuildNetCore")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build");

// The default task to run if none is explicitly specified. In this case, we want
// to run everything starting from Clean, all the way up to Publish.
Task("Default")
    .IsDependentOn("SonarBegin")
    .IsDependentOn("BuildNetCore")
    .IsDependentOn("TestNetCore")
    .IsDependentOn("SonarEnd")
    .IsDependentOn("PublishWeb")
    .IsDependentOn("CoverageReport")
    .IsDependentOn("Zip");

// To get fast all (net core) assemblies
// ./build.sh --Runtime=osx.10.12-x64 --Target=BuildPublishWithoutTest
Task("BuildPublishWithoutTest")
    .IsDependentOn("BuildNetCore")
    .IsDependentOn("PublishWeb");

// Executes the task specified in the target argument.
RunTarget(target);
