using Microsoft.EntityFrameworkCore;

namespace ScaffoldingCore
{
    public class MyContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("empty");
    }

    public class Log
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}