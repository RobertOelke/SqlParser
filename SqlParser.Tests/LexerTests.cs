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

    [Fact]
    public void LexQuotedText()
    {
        var testQuery = "SELECT 'Hello World' AS Text FROM DUAL";
        var lexer = new Lexer(testQuery);
        Assert.Equal(SyntaxKind.SelectToken, lexer.NextToken().Kind);
        Assert.Equal(SyntaxKind.WhitespaceToken, lexer.NextToken().Kind);
        var quotedText = lexer.NextToken();
        var helloWorld = quotedText.Text(testQuery);
        Assert.Equal(SyntaxKind.QuotedTextToken, quotedText.Kind);
        Assert.Equal("'Hello World'", helloWorld);
        Assert.Equal(SyntaxKind.WhitespaceToken, lexer.NextToken().Kind);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("-1")]
    [InlineData("1.2")]
    [InlineData("-1.2")]
    [InlineData("100")]
    [InlineData("100.200")]
    public void LexNumber(string testNumber)
    {
        var lexer = new Lexer(testNumber);
        var parsedNumber = lexer.NextToken();
        Assert.Equal(SyntaxKind.NumberToken, parsedNumber.Kind);
        Assert.Equal(testNumber.Length, parsedNumber.Length);
    }
}