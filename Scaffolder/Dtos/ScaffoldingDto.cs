using System.Collections.Generic;
using System.Collections.Immutable;

namespace Scaffolding.Dtos
{
    public class ScaffoldingDto
    {
        public string DbContextSource { get; set; }
        public IReadOnlyList<string> ModelSources { get; set; } = new List<string>();
        public IReadOnlyList<string> AllFiles => ModelSources.ToImmutableList().Add(DbContextSource);
    }
}