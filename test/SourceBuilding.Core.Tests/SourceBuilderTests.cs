using System.IO;
using Xunit;

namespace SourceBuilding.Core.Tests
{
    public class SourceBuilderTests
    {
        [Fact]
        public void BuildAssemblyTest()
        {
            var builder = new SourceBuilder();
            var result = builder.BuildAssembly(new string[0]);
        }

        [Fact]
        public void BuildScriptTest()
        {
            const string source1 =
@"namespace TestNamespace
{
    public class TestClass
    {

    }
}";
            const string source2 =
@"namespace TestNamespace
{
    public class TestClass2
    {

    }

    public class TestClass3
    {
        public void DoNothing()
        {

        }
    }
}";
            var builder = new SourceBuilder();
            var result = builder.BuildAssembly(new[] { source1, source2 });
            File.WriteAllBytes(@"C:\Users\sharpiro\AppData\Local\SqlMapper\testAssembly.dll", result);
        }
    }
}