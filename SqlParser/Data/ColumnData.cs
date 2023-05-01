namespace SqlParser.Data;

public enum ColumnType
{
    Varchar2,
    Number,
    Date,
}


public sealed record ColumnData(
    int ColumnId,
    string Name,
    ColumnType DataType,
    int? DataLength,
    int? DataScale,
    int? DataPrecision,
    int? CharLength,
    bool Nullable);