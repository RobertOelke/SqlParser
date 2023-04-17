namespace SqlParser;

public record struct SyntaxToken(SyntaxKind Kind, int Start, int Length, string? Text);
