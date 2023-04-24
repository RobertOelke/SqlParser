namespace SqlParser.Parsing;

public enum ExpressionKind
{
    // Keywords
    SelectKeyword,
    FromKeyword,
    WhereKeyword,

    Identifier,

    ColumnListExpression,
    ColumnExpression,

    Unparsed,
    EndOfBatch,
}

public abstract class SqlExpression
{
    protected SqlExpression(ExpressionKind kind, int start, int length)
    {
        Kind = kind;
        Start = start;
        Length = length;
    }

    public ExpressionKind Kind { get; }
    public int Start { get; }
    public int Length { get; }
}

public sealed class KeywordExpression : SqlExpression
{
    public KeywordExpression(ExpressionKind kind, int start, int length) : base(kind, start, length)
    {
    }
}

public sealed class IdentifierExpression : SqlExpression
{
    public IdentifierExpression(int start, int length) : base(ExpressionKind.Identifier, start, length)
    {
    }
}

public class ColumnExpression : SqlExpression
{
    public ColumnExpression(int start, int length) : base(ExpressionKind.ColumnExpression, start, length)
    {
    }
}

public sealed class ColumnIdentifierExpression : ColumnExpression
{
    public ColumnIdentifierExpression(IdentifierExpression identifier) : base(identifier.Start, identifier.Length)
    {
        Identifier = identifier;
    }

    public IdentifierExpression Identifier { get; }
}

public sealed class InvalidColumnExpression : ColumnExpression
{
    public InvalidColumnExpression(int start, int length) : base(start, length)
    {
    }
}

public sealed class ColumnListExpression : SqlExpression
{
    public ColumnListExpression(List<ColumnExpression> columns)
        : base(
            ExpressionKind.ColumnListExpression,
            columns.First().Start,
            columns.Last().Start - columns.First().Start + columns.Last().Length)
    {
        Items = columns;
    }

    public List<ColumnExpression> Items { get; }
}

public sealed class EndOfBatchExpression : SqlExpression
{
    public EndOfBatchExpression(int start) : base(ExpressionKind.EndOfBatch, start, 0)
    {   
    }
}

public sealed class UnparsedExpression : SqlExpression
{
    public UnparsedExpression(int start, int length) : base(ExpressionKind.Unparsed, start, length)
    {
    }
}
