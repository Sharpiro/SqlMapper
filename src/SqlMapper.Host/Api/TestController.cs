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

        public async Task<object> Get(string connectionString, string databaseName, string workspaceDir, LibType libType)
        {
            await Task.Yield();
            try
            {
                //var libType = LibType.Assembly;
                //var libType = LibType.Script;

                var scaffolder = new Scaffolder();
                var sourceBuilder = new SourceBuilder();
                var scriptBuilder = new ScriptGenerator();

                var @namespace = libType == LibType.Assembly ? "GeneratedNamespace" : "System";
                var contextName = $"{databaseName}Context";

                var userFolder = Environment.GetEnvironmentVariable("LocalAppData");
                var appFolder = $"{userFolder}\\SqlMapper";
                var directory = new DirectoryInfo(appFolder);
                if (!directory.Exists) directory.Create();
                var libraryPath = libType == LibType.Assembly ? $"{appFolder}\\generatedAssembly.dll" : $"{appFolder}\\generatedAssembly.csx";
                var scriptPath = $"{workspaceDir}\\main.csx";

                var queryableExtensionsSource = File.ReadAllText($"{System.AppContext.BaseDirectory}/IQueryableExtensions.cs");
                var scaffolding = scaffolder.ScaffoldDatabase(connectionString, @namespace, contextName);
                scaffolding.AdditionalFiles.Add(queryableExtensionsSource);

                var libraryData = sourceBuilder.Build(scaffolding.AllFiles, libType);
                var firstDbsetPropertyName = scriptBuilder.GetPropertyName(scaffolding.DbContextSource);
                var script = scriptBuilder.BuildMainScript(@namespace, contextName, libraryPath, firstDbsetPropertyName);

                File.WriteAllBytes(libraryPath, libraryData);
                File.WriteAllText(scriptPath, script);

                return new { ScriptPath = scriptPath };
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