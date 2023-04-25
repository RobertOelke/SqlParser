namespace SqlParser.Parsing;

public enum SqlClauseKind
{
    SelectClause,
    FromClause,
    WhereClause,

    EndOfBatchClause,
    UnparsableClause,
}

public abstract class SqlClause
{
    public abstract SqlClauseKind Kind { get; }
}

public sealed class SelectClause : SqlClause
{
    public SelectClause(KeywordExpression selectKeyword, ColumnListExpression columnList)
    {
        SelectKeyword = selectKeyword;
        ColumnList = columnList;
    }

    public override SqlClauseKind Kind => SqlClauseKind.SelectClause;

    public KeywordExpression SelectKeyword { get; }

    public ColumnListExpression ColumnList { get; }
}

public sealed class FromClause : SqlClause
{
    public FromClause(KeywordExpression fromKeyword, TableExpression mainTable)
    {
        FromKeyword = fromKeyword;
        MainTable = mainTable;
    }

    public override SqlClauseKind Kind => SqlClauseKind.FromClause;

    public KeywordExpression FromKeyword { get; }

    public TableExpression MainTable { get; }
}

public sealed class WhereClause : SqlClause
{
    public WhereClause(KeywordExpression whereKeyword, BooleanExpression rootExpression)
    {
        WhereKeyword = whereKeyword;
        RootExpression = rootExpression;
    }

    public override SqlClauseKind Kind => SqlClauseKind.WhereClause;

    public KeywordExpression WhereKeyword { get; }

    public BooleanExpression RootExpression { get; }
}

public sealed class EndOfBatchClause : SqlClause
{
    public EndOfBatchClause(EndOfBatchExpression endOfBatch)
    {
        EndOfBatch = endOfBatch;
    }

    public override SqlClauseKind Kind => SqlClauseKind.EndOfBatchClause;

    public EndOfBatchExpression EndOfBatch { get; }
}

public sealed class UnparsedClause : SqlClause
{
    public UnparsedClause(UnparsedExpression unparsed)
    {
        Unparsed = unparsed;
    }

    public override SqlClauseKind Kind => SqlClauseKind.UnparsableClause;

    public UnparsedExpression Unparsed { get; }
}