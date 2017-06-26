using System.Threading.Tasks;
using Scaffolding;

namespace SqlMapper.Host.Api
{
    public class TestController
    {
        public async Task<object> Get(string connectionString, string databaseName)
        {
            await Task.Yield();
            var scaffolder = new Scaffolder();
            var scaffolding = scaffolder.ScaffoldDatabase(connectionString, "GeneratedNamespace", $"{databaseName}Context");
            //var sourceBuilder = new SourceBuilder();
            //sourceBuilder.Build(scaffolding.AllFiles);
            return new { Data = "testData" };
        }
    }
}