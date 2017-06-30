using Xunit;

namespace SourceBuilding.Core.Tests
{
    public class SourceBuilderTests
    {
        [Fact]
        public void BuildTest()
        {
            var builder = new SourceBuilder();
            var result = builder.Build(new string[0]);
        }
    }
}