namespace SqlParser.Tokenization;

using System.Text;

public static class TokenPrinter
{
    public static string Print(string src, IEnumerable<SyntaxToken> tokens)
    {
        var sb = new StringBuilder();

        foreach (var token in tokens)
        {
            switch (token.Kind)
            {
                case SyntaxKind.SelectToken:
                case SyntaxKind.FromToken:
                case SyntaxKind.WhereToken:
                case SyntaxKind.AsToken:
                case SyntaxKind.LiteralToken:
                    sb.Append(token.Text(src).ToUpperInvariant());
                    break;
                case SyntaxKind.NumberToken:
                case SyntaxKind.QuotedTextToken:
                case SyntaxKind.BadIdentifierToken:
                case SyntaxKind.EqualsToken:
                case SyntaxKind.CommaToken:
                    sb.Append(token.Text(src));
                    break;
                case SyntaxKind.WhitespaceToken:
                    sb.Append(' ');
                    break;
                case SyntaxKind.EndOfFileToken:
                default:
                    break;
            }
        }

        return sb.ToString();
    }

}

