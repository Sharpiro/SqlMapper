using Xunit;

namespace SourceBuilding.Core.Tests
{
    public class ScriptBuilderTests
    {
        [Fact]
        public void BuildTest()
        {
            var scriptBuilder = new ScriptBuilder();
            var script = scriptBuilder.Build("GeneratedNamespace", "GeneratedContext", @"C:\temp\temp.dll", "Tacos");
        }

        [Fact]
        public void GetPropertyNameTest()
        {
            var source =
@"public class TestContext
{
    public DbSet<Log> Logs { get; set; }
}";
            var scriptBuilder = new ScriptBuilder();

            var propertyName = scriptBuilder.GetPropertyName(source);

            Assert.Equal("Logs", propertyName);
        }
    }
}