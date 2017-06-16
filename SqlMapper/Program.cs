using System;
using System.Linq;
using SqlMapper.Core;

namespace SqlMapper
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            const string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Health;Trusted_Connection=True;MultipleActiveResultSets=true";
            var service = new SchemaService(connectionString);
            var databases = service.GetDatabasesNames();
            var dbName = databases.Single(dn => dn.Equals("health", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}