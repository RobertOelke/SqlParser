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
    AsToken,

    // Operators
    EqualsToken,
    CommaToken,

    BadIdentifierToken,
    EndOfFileToken,
}