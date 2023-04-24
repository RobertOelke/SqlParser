namespace SqlParser.Tokenization;

public record struct SyntaxToken(SyntaxKind Kind, int Start, int Length);

public static class SyntaxTokenExtentions
{
    public static bool IsColumnSeparator(this SyntaxToken token)
    {
        return token.Kind switch
        {
            SyntaxKind.CommaToken => true,
            _ => false,
        };
    }
}