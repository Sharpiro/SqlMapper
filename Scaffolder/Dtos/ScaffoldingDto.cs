using System.Collections.Generic;

namespace Scaffolding.Dtos
{
    public class ScaffoldingDto
    {
        public string DbContextSource { get; set; }
        public IReadOnlyList<string> ModelSources { get; set; } = new List<string>();
    }
}