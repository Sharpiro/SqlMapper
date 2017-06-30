using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Scaffolding.CustomScaffolding
{
    /// <summary>
    /// SqlServerScaffoldingModelFactory reads the database schema 
    /// and turns it into an EF model.
    /// 
    /// We override this so that we can singularize entity type 
    /// names in the model.
    /// </summary>
    public class SingularScaffoldingModelFactory : SqlServerScaffoldingModelFactory
    {
        public SingularScaffoldingModelFactory(
            ILoggerFactory loggerFactory,
            IRelationalTypeMapper typeMapper,
            IDatabaseModelFactory databaseModelFactory,
            CandidateNamingService candidateNamingService)
            : base(
                loggerFactory,
                typeMapper,
                databaseModelFactory,
                candidateNamingService)
        { }

        protected override string GetEntityTypeName(TableModel table)
        {
            // Use the base implementation to get a C# friendly name
            var name = base.GetEntityTypeName(table);

            // Singularize the name
            return Inflector.Inflector.Singularize(name) ?? name;
        }
    }
}
