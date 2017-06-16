using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Configuration.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.Extensions.Logging;

namespace Scaffolding
{
    public class Scaffolder
    {
        public void ScaffoldDatabase(string connectionString)
        {
            var scaffUtils = new ScaffoldingUtilities();
            var csharpUtils = new CSharpUtilities();
            //var writer = new CustomDbContextWriter(scaffUtils, csharpUtils);
            IModel model = new Model();
            var scaffolding = model.Scaffolding();
            scaffolding.UseProviderMethodName = "Microsoft.EntityFrameworkCore.SqlServer";
            //scaffolding.DatabaseName = "localhost";
            //scaffolding.DefaultSchema = "dbo";
            var provider = new SqlServerAnnotationProvider();
            //var customConfigFactory = new CustomConfigurationFactory(provider, csharpUtils, scaffUtils);
            var customConfiguration = new CustomConfiguration(connectionString, "GeneratedContext", "ConsoleApp2", true);
            //var modelConfig = customConfigFactory.CreateModelConfiguration(model, configX);
            //IEntityType entityType = new EntityType("Table", (Model)model, ConfigurationSource.Convention);
            //config.AddTableNameConfiguration(new EntityConfiguration(config, entityType));
            //var entityConfig = new EntityConfiguration(modelConfig, entityType);
            //var xyz = modelConfig.AnnotationProvider;
            //var result = writer.WriteCode(modelConfig);

            var entityTypeWriter = new EntityTypeWriter(csharpUtils);
            //var code = entityTypeWriter.WriteCode(entityConfig);

            var dbContextWriter = new DbContextWriter(scaffUtils, csharpUtils);
            //var @out = dbContextWriter.WriteCode(modelConfig);

            var factory = new ConfigurationFactory(provider, csharpUtils, scaffUtils);
            //factory.CreateEntityConfiguration(modelConfig, )
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<SqlServerDatabaseModelFactory>();
            var revEngeConfig = new ReverseEngineeringConfiguration
            {
                ConnectionString = connectionString,
                ProjectPath = @"C:\Users\U403598\Desktop\temp\ef_out",
                ProjectRootNamespace = "ConsoleApp2"
            };
            var sqlScaffoldModelFactory = new SqlServerScaffoldingModelFactory(new LoggerFactory(), new SqlServerTypeMapper(), new SqlServerDatabaseModelFactory(logger), new CandidateNamingService());
            var codeWriter = new StringBuilderCodeWriter(new FileSystemFileService(), dbContextWriter, entityTypeWriter);

            var generator = new ReverseEngineeringGenerator(sqlScaffoldModelFactory, factory, codeWriter);
            var metaModel = generator.GetMetadataModel(revEngeConfig);
            //var data = generator.GenerateAsync(revEngeConfig).Result;

            var modelConfiguration = factory.CreateModelConfiguration(metaModel, customConfiguration);
            var dbContextData = dbContextWriter.WriteCode(modelConfiguration);
            var files = new List<string> { dbContextData };
            foreach (var entityConfiguration in modelConfiguration.EntityConfigurations)
            {
                var temp2 = entityTypeWriter.WriteCode(entityConfiguration);
                files.Add(temp2);
            }
        }
    }
}