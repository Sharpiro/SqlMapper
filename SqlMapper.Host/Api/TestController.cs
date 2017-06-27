using System;
using System.IO;
using System.Threading.Tasks;
using Scaffolding;
using SourceBuilding.Core;

namespace SqlMapper.Host.Api
{
    public class TestController
    {
        public async Task<object> Get(string connectionString, string databaseName)
        {
            await Task.Yield();
            try
            {
                var scaffolder = new Scaffolder();
                var sourceBuilder = new SourceBuilder();
                var scriptBuilder = new ScriptBuilder();

                const string @namespace = "GeneratedNamespace";
                var contextName = $"{databaseName}Context";

                var userFolder = Environment.GetEnvironmentVariable("LocalAppData");
                var appFolder = $"{userFolder}\\SqlMapper";
                var directory = new DirectoryInfo(appFolder);
                if (!directory.Exists) directory.Create();
                var assemblyPath = $"{appFolder}\\generatedAssembly.dll";

                var scaffolding = scaffolder.ScaffoldDatabase(connectionString, @namespace, contextName);
                var assemblyBytes = sourceBuilder.Build(scaffolding.AllFiles);
                var firstDbsetPropertyName = scriptBuilder.GetPropertyName(scaffolding.DbContextSource);
                var script = scriptBuilder.Build(@namespace, contextName, assemblyPath, firstDbsetPropertyName);

                File.WriteAllBytes(assemblyPath, assemblyBytes);

                var dto = new { Script = script };

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