using System.Collections.Immutable;

namespace SqlParser.Data;

public sealed class TableData
{
    public TableData(string name, ImmutableList<ColumnData> columns)
    {
        Name = name;
        Columns = columns;
    }

    public string Name { get; }

    public ImmutableList<ColumnData> Columns { get; }
}
