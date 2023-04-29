namespace SqlParser.Data;

public sealed class ColumnData
{
    public ColumnData(string name)
    {
        Name = name;
    }

    public string Name { get; }
}