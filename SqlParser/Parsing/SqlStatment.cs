namespace SqlParser.Parsing;

public enum StatmentKind
{
    SelectStatement,
    EndOfBatch,
}

public abstract class SqlStatment
{
    public abstract StatmentKind Kind { get; }

}

public sealed class SqlSelectStatement : SqlStatment
{
    public override StatmentKind Kind => StatmentKind.SelectStatement;
}

public sealed class EndOfBatchStatement : SqlStatment
{
    public override StatmentKind Kind => StatmentKind.EndOfBatch;
}