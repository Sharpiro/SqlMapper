using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace SqlMapper.Core.Dtos
{
    public class TableDto
    {
        public string Name { get; private set; }
        public IEnumerable<ColumnDto> Columns { get; private set; } = new List<ColumnDto>();

        public TableDto WithName(string name)
        {
            var dto = Clone();
            dto.Name = name;
            return dto;
        }

        public TableDto Clone()
        {
            return new TableDto
            {
                Name = Name,
                Columns = Columns.Select(c => c.Clone()).ToImmutableList()
            };
        }

        public TableDto AddColumns(params ColumnDto[] columns)
        {
            var clone = Clone();
            clone.Columns = clone.Columns.Concat(columns.Select(c => c.Clone()));
            return clone;
        }

        internal TableDto WithColumns(IEnumerable<ColumnDto> columns)
        {
            var clone = Clone();
            clone.Columns = columns.Select(c => c.Clone());
            return clone;
        }
    }
}