namespace SqlParser.Tests;

public static class LexerTestsData
{
    public static IEnumerable<object[]> GetKeywords()
    {
        yield return new object[] { "SELECT", SyntaxKind.SelectToken };
        yield return new object[] { "select", SyntaxKind.SelectToken };
        yield return new object[] { "sELEct", SyntaxKind.SelectToken };
        yield return new object[] { "sELEct123", SyntaxKind.LiteralToken };

        yield return new object[] { "WHERE", SyntaxKind.WhereToken };
        yield return new object[] { "FROM", SyntaxKind.FromToken };
    }
}