using System.Collections.Immutable;

namespace SqlParser.Binding;

public sealed record BoundSelectStatement(
    ImmutableList<BoundColumn> Selection)
{
    public static BoundSelectStatement Empty { get; } = new BoundSelectStatement(ImmutableList<BoundColumn>.Empty);
}