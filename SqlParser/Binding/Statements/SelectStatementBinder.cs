using SqlParser.Data;
using SqlParser.Parsing;
using System.Collections.Immutable;

namespace SqlParser.Binding.Statements;

internal static class SelectStatementBinder
{
    public static BoundSelectStatement BindSelect(BinderContext<SelectStatementData> context)
    {
        var selectionBuilder = ImmutableList.CreateBuilder<BoundColumn>();

        var columns = GetBoundColumns(context);

        var selectionList = selectionBuilder.ToImmutableList();

        for (int i = 0; i < context.Statement.Select.ColumnList.Items.Count; i++)
        {
            var col = context.Statement.Select.ColumnList.Items[i];
            var colName = col.Text(context.Src).ToUpperInvariant();

            var colData = columns.FirstOrDefault(c => string.Equals(c.Name, colName, StringComparison.InvariantCultureIgnoreCase));

            var boundData = new BoundColumn(i, col.Text(context.Src).ToUpperInvariant(), colData ?? new ColumnData("<unknown>"));

            if (colData == null)
            {
                context.Diagnostics.ReportColumnNotBound(col.Start, col.Length);
            }
        }

        return new BoundSelectStatement(selectionList);
    }

    public static IEnumerable<ColumnData> GetBoundColumns(BinderContext<SelectStatementData> context)
    {
        var tableName =
            context.Statement.From.MainTable switch
            {
                NamedTableExpression namedTable => namedTable.Text(context.Src),
                _ => ""
            };

        var boundTable = context.DataContext.Tables.FirstOrDefault(t => string.Equals(t.Name, tableName, StringComparison.InvariantCultureIgnoreCase));

        if (boundTable == null)
        {
            context.Diagnostics.ReportTableNotBound(context.Statement.From.MainTable.Start, context.Statement.From.MainTable.Length);
        }

        return boundTable?.Columns ?? Enumerable.Empty<ColumnData>();
    }
}
