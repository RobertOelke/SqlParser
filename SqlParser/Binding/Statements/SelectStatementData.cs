using SqlParser.Parsing;

namespace SqlParser.Binding.Statements;

internal sealed record SelectStatementData(
    SelectClause Select,
    FromClause From,
    WhereClause? Where);