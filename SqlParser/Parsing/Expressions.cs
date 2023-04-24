namespace SqlParser.Parsing;

public enum ExpressionKind
{
    // Keywords
    SelectKeyword,
    FromKeyword,
    WhereKeyword,
    AsKeyword,

    Identifier,
    QuotedText,
    Number,

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

    public string Text(string src) => src.Substring(Start, Length);
}

public sealed class KeywordExpression : SqlExpression
{
    public KeywordExpression(ExpressionKind kind, int start, int length) : base(kind, start, length)
    {
    }
}
public sealed class QuotedTextExpression : SqlExpression
{
    public QuotedTextExpression(int start, int length) : base(ExpressionKind.QuotedText, start, length)
    {
    }
}

public sealed class NumberExpression : SqlExpression
{
    public NumberExpression(int start, int length) : base(ExpressionKind.Number, start, length)
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

public sealed class ColumnQuotedTextExpresstion : ColumnExpression
{
    public ColumnQuotedTextExpresstion(
        QuotedTextExpression quotedText,
        IdentifierExpression alias,
        KeywordExpression? asKeyword)
        : base(quotedText.Start, alias.Start + alias.Length - quotedText.Start)
    {
        QuotedText = quotedText;
        Alias = alias;
        AsKeyword = asKeyword;
    }

    public QuotedTextExpression QuotedText { get; }
    public IdentifierExpression Alias { get; }
    public KeywordExpression? AsKeyword { get; }
}

public sealed class ColumnConstantNumberExpression : ColumnExpression
{
    public ColumnConstantNumberExpression(
        NumberExpression number,
        IdentifierExpression alias,
        KeywordExpression? asKeyword)
        : base(number.Start, alias.Start + alias.Length - number.Start)
    {
        Number = number;
        Alias = alias;
        AsKeyword = asKeyword;
    }

    public NumberExpression Number { get; }
    public IdentifierExpression Alias { get; }
    public KeywordExpression? AsKeyword { get; }
}

public sealed class ColumnAliasedIdentifierExpression : ColumnExpression
{
    public ColumnAliasedIdentifierExpression(
        IdentifierExpression identifier,
        IdentifierExpression alias,
        KeywordExpression? asKeyword)
        : base(identifier.Start, alias.Start + alias.Length - identifier.Start)
    {
        Identifier = identifier;
        AsKeyword = asKeyword;
        Alias = alias;
    }

    public IdentifierExpression Identifier { get; }
    public IdentifierExpression Alias { get; }
    public KeywordExpression? AsKeyword { get; }
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
