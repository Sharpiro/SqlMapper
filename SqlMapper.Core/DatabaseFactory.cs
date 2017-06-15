using SqlMapper.Core.Dtos;

namespace SqlMapper.Core
{
    public static class DatabaseFactory
    {
        public static DatabaseDto DatabaseDto(string name = null)
        {
            var dto = new DatabaseDto();
            if (!string.IsNullOrEmpty(name))
                dto = dto.WithName(name);
            return dto;
        }

        public static TableDto TableDto(string name = null)
        {
            var dto = new TableDto();
            if (!string.IsNullOrEmpty(name))
                dto = dto.WithName(name);
            return dto;
        }

        public static ColumnDto ColumnDto(string name = null)
        {
            var dto = new ColumnDto();
            if (!string.IsNullOrEmpty(name))
                dto = dto.WithName(name);
            return dto;
        }
    }
}