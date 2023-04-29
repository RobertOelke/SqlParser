using SqlParser.Binding.Statements;
using SqlParser.Data;
using SqlParser.Diagnostics;
using SqlParser.Parsing;

namespace SqlParser.Binding;

public sealed class Binder
{
    private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

    private readonly DataContext _dataContext;
    private readonly Parser _parser;
    private readonly string _src;

    public Binder(DataContext dataContext, Parser parser, string src)
    {
        _dataContext = dataContext;
        _parser = parser;
        _src = src;
    }

    public BoundSelectStatement BindSelect()
    {
        var clauses = ReadToEnd().ToArray();

        if (clauses.Length < 1 || clauses[0] is not SelectClause selectClause)
        {
            _diagnostics.ReportSelectClauseNotFound(0, _src.Length);
            return BoundSelectStatement.Empty;
        }

        if (clauses.Length < 2 || clauses[1] is not FromClause fromClause)
        {
            _diagnostics.ReportFromClauseNotFound(0, _src.Length);
            return BoundSelectStatement.Empty;
        }

        var whereClause = clauses[2] as WhereClause;

        var data = new SelectStatementData(selectClause, fromClause, whereClause);

        var context = new BinderContext<SelectStatementData>(_src, _diagnostics, _dataContext, data);

        return SelectStatementBinder.BindSelect(context);
    }

    private IEnumerable<SqlClause> ReadToEnd()
    {
        while (_parser.NextClause() is var clause)
        {
            yield return clause;

            if (clause.Kind == SqlClauseKind.EndOfBatchClause)
                yield break;
        }
    }
}