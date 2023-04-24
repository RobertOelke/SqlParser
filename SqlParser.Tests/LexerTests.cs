namespace SqlParser.Tests;

using SqlParser.Tokenization;

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

    [Fact]
    public void SimpleQuery()
    {
        var testQuery = "SELECT Field, Field2 FROM Table WHERE Field = 2";

        var lexer = new Lexer(testQuery);
        Assert.Equal(SyntaxKind.SelectToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.WhitespaceToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.LiteralToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.CommaToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.WhitespaceToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.LiteralToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.WhitespaceToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.FromToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.WhitespaceToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.LiteralToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.WhitespaceToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.WhereToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.WhitespaceToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.LiteralToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.WhitespaceToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.EqualsToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.WhitespaceToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.NumberToken, lexer.NextToken().Kind);
    }
}