using SqlParser.Data;

namespace SqlParser.Binding;

public sealed record BoundColumn(
    int Index,
    string Name,
    ColumnData? BoundData);