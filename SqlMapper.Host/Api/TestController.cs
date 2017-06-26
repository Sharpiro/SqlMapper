using System;
using System.IO;
using System.Threading.Tasks;
using Scaffolding;
using SourceBuilding.Core;

namespace SqlMapper.Host.Api
{
    public class TestController
    {
        public async Task<string> Get(string connectionString, string databaseName)
        {
            try
            {
                await Task.Yield();
                var scaffolder = new Scaffolder();
                var scaffolding = scaffolder.ScaffoldDatabase(connectionString, "GeneratedNamespace", $"{databaseName}Context");
                var sourceBuilder = new SourceBuilder();
                var assemblyBytes = sourceBuilder.Build(scaffolding.AllFiles);

                var userFolder = Environment.GetEnvironmentVariable("LocalAppData");
                var appFolder = $"{userFolder}\\SqlMapper";

                var directory = new DirectoryInfo(appFolder);
                if (!directory.Exists) directory.Create();
                var outFilePath = $"{appFolder}\\generatedAssembly.dll";

                File.WriteAllBytes(outFilePath, assemblyBytes);

                return outFilePath;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}