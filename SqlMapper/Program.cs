using System;
using System.Linq;
using SqlMapper.Core;

namespace SqlMapper
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            const string connectionString = "Server=localhost;Database=ZLUtilities;Trusted_Connection=True;";
            var service = new SchemaService(connectionString);
            var databases = service.GetDatabasesNames();
            var dbName = databases.Single(dn => dn.Equals("health", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}