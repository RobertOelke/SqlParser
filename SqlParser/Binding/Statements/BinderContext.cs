using SqlParser.Data;
using SqlParser.Diagnostics;

namespace SqlParser.Binding.Statements;

internal sealed record BinderContext<TStatementData>(
    string Src,
    DiagnosticBag Diagnostics,
    DataContext DataContext,
    TStatementData Statement);
