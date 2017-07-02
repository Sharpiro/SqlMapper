using System;
using System.IO;
using System.Threading.Tasks;
using Scaffolding;
using SourceBuilding.Core;
using Microsoft.AspNetCore.Hosting;

namespace SqlMapper.Host.Api
{
    public class TestController
    {
        private readonly IApplicationLifetime _appLifeTime;

        public TestController(IApplicationLifetime appLifeTime)
        {
            _appLifeTime = appLifeTime ?? throw new ArgumentNullException(nameof(appLifeTime));
        }

        public async Task<object> Get(string connectionString, string databaseName, string workspaceDir)
        {
            await Task.Yield();
            try
            {
                var scaffolder = new Scaffolder();
                var sourceBuilder = new SourceBuilder();
                var scriptBuilder = new ScriptGenerator();

                const string @namespace = "GeneratedNamespace";
                //const string @namespace = "System";
                var contextName = $"{databaseName}Context";

                var userFolder = Environment.GetEnvironmentVariable("LocalAppData");
                var appFolder = $"{userFolder}\\SqlMapper";
                var directory = new DirectoryInfo(appFolder);
                if (!directory.Exists) directory.Create();
                var libraryPath = $"{appFolder}\\generatedAssembly.dll";
                //var libraryPath = $"{appFolder}\\generatedAssembly.csx";
                var scriptPath = $"{workspaceDir}\\main.csx";

                //var queryableExtensionsSource = File.ReadAllText($"{System.AppContext.BaseDirectory}/IQueryableExtensions.cs");
                //scaffolding.AdditionalFiles.Add(queryableExtensionsSource);

                var scaffolding = scaffolder.ScaffoldDatabase(connectionString, @namespace, contextName);
                var libraryData = sourceBuilder.BuildAssembly(scaffolding.AllFiles);
                //var libraryData = sourceBuilder.BuildScript(scaffolding.AllFiles);
                var firstDbsetPropertyName = scriptBuilder.GetPropertyName(scaffolding.DbContextSource);
                var script = scriptBuilder.BuildMainScript(@namespace, contextName, libraryPath, firstDbsetPropertyName);

                File.WriteAllBytes(libraryPath, libraryData);
                //File.WriteAllText(libraryPath, libraryData);
                File.WriteAllText(scriptPath, script);

                var dto = new { ScriptPath = scriptPath };

                return dto;
            }
#pragma warning disable 168
            catch (Exception ex)
#pragma warning restore 168
            {
                throw;
            }
        }
    }
}