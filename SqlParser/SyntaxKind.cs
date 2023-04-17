namespace SqlParser;

public enum SyntaxKind
{
    LiteralToken,
    NumberToken,
    WhitespaceToken,

    // Keywords
    SelectToken,
    FromToken,
    WhereToken,

    // Operators
    EqualsToken,
    CommaToken,

    BadIdentifierToken,
    EndOfFileToken,
}
