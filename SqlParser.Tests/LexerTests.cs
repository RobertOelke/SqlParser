namespace SqlParser.Tests;

using SqlParser;

public class LexerTests
{
    [Theory]
    [MemberData(nameof(GetKeywords))]
    public void KeywordTests(string testString, SyntaxKind expectedKeyword)
    {
        var lexer = new Lexer(testString);

        var parsedToken = lexer.NextToken();

        Assert.Equal(expectedKeyword, parsedToken.Kind);
    }

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