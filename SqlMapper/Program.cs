using System;
using System.Linq;
using Scaffolding;
using SqlMapper.Core;

namespace SqlMapper
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            const string baseConnString = "Server=localhost;Database={0};Trusted_Connection=True;";
            //const string baseConnString = @"Server=(localdb)\mssqllocaldb;Database={0};Trusted_Connection=True;";
            const string outDir = @"C:\Users\U403598\Desktop\temp\ef_out";
            const string rootNamespace = "GeneratedNamespace";
            const string contextName = "GeneratedContext";
            var masterConnString = string.Format(baseConnString, "master");
            var schemaService = new SchemaService(masterConnString);
            var scaffolder = new Scaffolder();
            var databases = schemaService.GetDatabasesNames();
            var dbName = databases.Single(dn => dn.Equals("zlutilities", StringComparison.InvariantCultureIgnoreCase));
            var dbConnString = string.Format(baseConnString, dbName);
            var scaffoldingDto = scaffolder.ScaffoldDatabase(dbConnString, outDir, rootNamespace, contextName);
            var joined = string.Join(Environment.NewLine, scaffoldingDto.ModelSources);
        }
    }
}