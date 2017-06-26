using System.Collections.Immutable;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding.Configuration.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.Extensions.Logging;
using Scaffolding.CustomScaffolding;
using Scaffolding.Dtos;

namespace Scaffolding
{
    public class Scaffolder
    {
        public ScaffoldingDto ScaffoldDatabase(string connectionString, string rootNamespace, string contextName)
        {
            var scaffUtils = new ScaffoldingUtilities();
            var csharpUtils = new CSharpUtilities();
            var provider = new SqlServerAnnotationProvider();
            var customConfiguration = new CustomConfiguration(connectionString, contextName, rootNamespace, true);
            var entityTypeWriter = new EntityTypeWriter(csharpUtils);
            var dbContextWriter = new SingularDbContextWriter(scaffUtils, csharpUtils);
            var configurationFactory = new ConfigurationFactory(provider, csharpUtils, scaffUtils);
            var loggerFactory = new LoggerFactory();
            var logger = loggerFactory.CreateLogger<SqlServerDatabaseModelFactory>();
            var revEngeConfig = new ReverseEngineeringConfiguration { ConnectionString = connectionString };
            var sqlScaffoldModelFactory = new SingularScaffoldingModelFactory(new LoggerFactory(),
                new SqlServerTypeMapper(), new SqlServerDatabaseModelFactory(logger), new CandidateNamingService());
            var codeWriter = new StringBuilderCodeWriter(new FileSystemFileService(), dbContextWriter, entityTypeWriter);

            var generator = new ReverseEngineeringGenerator(sqlScaffoldModelFactory, configurationFactory, codeWriter);
            var metaModel = generator.GetMetadataModel(revEngeConfig);
            var modelConfiguration = configurationFactory.CreateModelConfiguration(metaModel, customConfiguration);
            var dbContextData = dbContextWriter.WriteCode(modelConfiguration);
            var scaffoldingDto = new ScaffoldingDto
            {
                DbContextSource = dbContextData,
                ModelSources = modelConfiguration.EntityConfigurations
                    .Select(entityConfiguration => entityTypeWriter.WriteCode(entityConfiguration))
                    .ToImmutableList()
            };
            return scaffoldingDto;
        }
    }
}