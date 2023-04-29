using System.Collections.Immutable;

namespace SqlParser.Binding.Statements;

internal static class SelectStatementBinder
{
    public static BoundSelectStatement BindSelect(BinderContext<SelectStatementData> context)
    {
        var selectionBuilder = ImmutableList.CreateBuilder<BoundColumn>();

        var selectionList = selectionBuilder.ToImmutableList();

        return new BoundSelectStatement(selectionList);
    }
}
