using Xunit;

namespace SourceBuilding.Core.Tests
{
    public class ScriptBuilderTests
    {
        [Fact]
        public void Test1()
        {
            var scriptBuilder = new ScriptBuilder();
            var script = scriptBuilder.Build("GeneratedNamespace", "GeneratedContext", @"C:\temp\temp.dll");
        }
    }
}