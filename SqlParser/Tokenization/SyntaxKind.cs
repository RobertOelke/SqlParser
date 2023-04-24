namespace SqlParser.Tokenization;

public enum SyntaxKind
{
    LiteralToken,
    NumberToken,
    // "Text ..."
    QuotedTextToken,
    // ToDo: 'Column Name'
    // QuotedIdentifierToken,
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