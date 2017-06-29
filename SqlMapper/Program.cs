using System;
using System.IO;
using System.Linq;
using Scaffolding;
using SourceBuilding;
using SqlMapper.Core;
using System.Data.Entity;

namespace SqlMapper
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            //const string baseConnString = "Server=localhost;Database={0};Trusted_Connection=True;";
            //const string baseConnString = @"Server=(localdb)\mssqllocaldb;Database={0};Trusted_Connection=True;";
            //const string rootNamespace = "GeneratedNamespace";
            //var masterConnString = string.Format(baseConnString, "master");
            //var schemaService = new SchemaService(masterConnString);
            //var scaffolder = new Scaffolder();
            //var sourceBuilder = new SourceBuilder();

            //var databases = schemaService.GetDatabasesNames();
            //var dbName = databases.Single(dn => dn.Equals("Temp", StringComparison.InvariantCultureIgnoreCase));
            //var contextName = $"{dbName}Context";
            //var dbConnString = string.Format(baseConnString, dbName);
            //var scaffoldingDto = scaffolder.ScaffoldDatabase(dbConnString, rootNamespace, contextName);
            //var allSourceText = string.Join("", scaffoldingDto.AllFiles);
            //var scaffoldedAssemblyBytes = sourceBuilder.Build(scaffoldingDto.AllFiles);

            //File.WriteAllBytes("C:\\temp\\gen.dll", scaffoldedAssemblyBytes);

            //var fileAssemblyBytes = File.ReadAllBytes(@"C:\Users\U403598\Desktop\temp\ef_out\test.dll");
            //var newAssembly = Assembly.Load(fileAssemblyBytes);
            //var types = newAssembly.GetTypes();
            TestEF();
        }

        private static void TestEF()
        {
            throw new NotImplementedException();
        }
    }
}