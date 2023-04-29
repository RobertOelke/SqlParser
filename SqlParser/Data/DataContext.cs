using System.Collections.Immutable;

namespace SqlParser.Data;

public sealed class DataContext
{
    public DataContext(ImmutableList<TableData> tables)
    {
        Tables = tables;
    }

    public ImmutableList<TableData> Tables { get; }
}
