using Microsoft.EntityFrameworkCore;
using System.Linq;
using Xunit;
using Scaffolding;

namespace SourceBuilding.Core.Tests
{
    public class EfTests
    {
        [Fact]
        public void EfTest()
        {
            var context = new TestContext();
            var query = context.Logs.Where(l => l.Id == 1);
            var sql = query.ToSql();
        }
    }

    public class TestContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=(localdb)\\mssqllocaldb;database=temp;trusted_connection=true");
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class Log
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}