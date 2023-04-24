namespace SqlParser.Tokenization;

public record struct SyntaxToken(SyntaxKind Kind, int Start, int Length);

public static class SyntaxTokenExtentions
{
    public static string Text(this SyntaxToken token, string src) =>
        src.Substring(token.Start, token.Length);

    public static bool IsColumnSeparator(this SyntaxToken token)
    {
        return token.Kind switch
        {
            SyntaxKind.CommaToken => true,
            _ => false,
        };
    }

    public static bool IsEndOfColumnExpression(this SyntaxToken token)
    {
        return token.Kind switch
        {
            SyntaxKind.CommaToken => true,
            SyntaxKind.FromToken => true,
            SyntaxKind.EndOfFileToken => true,
            _ => false,
        };
    }
}