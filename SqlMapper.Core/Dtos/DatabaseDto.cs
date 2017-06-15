using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SqlMapper.Core.Dtos
{
    public class DatabaseDto
    {
        public string Name { get; private set; }
        public IEnumerable<TableDto> Tables { get; private set; } = new List<TableDto>();

        public DatabaseDto WithName(string name)
        {
            return new DatabaseDto { Name = name };
        }

        public DatabaseDto WithTables(IEnumerable<TableDto> tables)
        {
            var clone = Clone();
            clone.Tables = tables.Select(t => t.Clone());
            return clone;
        }

        public DatabaseDto AddTables(params TableDto[] tables)
        {
            var clone = Clone();
            clone.Tables = clone.Tables.Concat(tables.Select(t => t.Clone()));
            return clone;
        }

        public DatabaseDto Clone()
        {
            return new DatabaseDto
            {
                Name = Name,
                Tables = Tables.Select(t => t.Clone()).ToImmutableList()
            };
        }
    }
}