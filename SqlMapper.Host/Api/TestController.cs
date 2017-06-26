using System.Threading.Tasks;

namespace SqlMapper.Host.Api
{
    public class TestController
    {
        public async Task<object> Get()
        {
            await Task.Yield();
            return new { Data = "testData" };
        }
    }
}