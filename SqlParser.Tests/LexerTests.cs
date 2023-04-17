namespace SqlParser.Tests;

using SqlParser;

public class LexerTests
{
    [Theory]
    [MemberData(nameof(LexerTestsData.GetKeywords), MemberType = typeof(LexerTestsData))]
    public void KeywordTests(string testString, SyntaxKind expectedKeyword)
    {
        var lexer = new Lexer(testString);

        var parsedToken = lexer.NextToken();

        Assert.Equal(expectedKeyword, parsedToken.Kind);
    }
}