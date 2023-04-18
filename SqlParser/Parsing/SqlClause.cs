using SqlParser.Tokenization;

namespace SqlParser.Parsing;

public enum SqlClauseKind
{
    SelectClause,
    FromClause,
    WhereClause,
    OrderClause,

    EndOfBatchClause,
}

public abstract class SqlClause
{
    public abstract SqlClauseKind Kind { get; }
}

public sealed class SelectClause : SqlClause
{
    public override SqlClauseKind Kind => SqlClauseKind.SelectClause;
}

public sealed class WhereClause : SqlClause
{
    public override SqlClauseKind Kind => SqlClauseKind.WhereClause;
}

public sealed class EndOfBatchClause : SqlClause
{
    public override SqlClauseKind Kind => SqlClauseKind.EndOfBatchClause;
}