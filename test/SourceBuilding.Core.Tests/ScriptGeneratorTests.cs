using Xunit;

namespace SourceBuilding.Core.Tests
{
    public class ScriptGeneratorTests
    {
        [Fact]
        public void BuildTest()
        {
            var scriptGenerator = new ScriptGenerator();
            //var script = scriptGenerator.BuildMainScript("GeneratedNamespace", "GeneratedContext", @"C:\temp\temp.dll", "Tacos");
            var script = scriptGenerator.BuildMainScript("System", "GeneratedContext", @"C:\temp\temp.csx", "Tacos");
        }

        [Fact]
        public void GetPropertyNameTest()
        {
            var source =
@"public class TestContext
{
    public DbSet<Log> Logs { get; set; }
}";
            var scriptBuilder = new ScriptGenerator();

            var propertyName = scriptBuilder.GetPropertyName(source);

            Assert.Equal("Logs", propertyName);
        }
    }
}