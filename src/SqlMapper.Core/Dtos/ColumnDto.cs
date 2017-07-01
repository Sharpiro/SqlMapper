namespace SqlMapper.Core.Dtos
{
    public class ColumnDto
    {
        public string Name { get; private set; }
        public string DataType { get; private set; }
        public int OrdinalPosition { get; private set; }

        public ColumnDto WithName(string name)
        {
            var newColumn = Clone();
            newColumn.Name = name;
            return newColumn;
        }

        public ColumnDto WithDataType(string dataType)
        {
            var newColumn = Clone();
            newColumn.DataType = dataType;
            return newColumn;
        }

        public ColumnDto WithOrdinalPosition(int ordinalPosition)
        {
            var newColumn = Clone();
            newColumn.OrdinalPosition = ordinalPosition;
            return newColumn;
        }

        public ColumnDto Clone()
        {
            return new ColumnDto
            {
                Name = Name,
                DataType = DataType,
                OrdinalPosition = OrdinalPosition
            };
        }
    }
}