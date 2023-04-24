using SqlParser.Tokenization;

namespace SqlParser.Parsing;

public sealed class Parser
{
    private readonly SyntaxToken[] _tokens;
    private readonly string _src;
    private int _position;
    
    public Parser(IEnumerable<SyntaxToken> tokens, string src)
    {
        _tokens = tokens.ToArray();
        _src = src;
    }

    private SyntaxToken Peek(int offset)
    {
        var index = _position + offset;

        if (index >= _tokens.Length)
            return new SyntaxToken(SyntaxKind.EndOfFileToken, 0, 0);
        
        return _tokens[index];
    }

    private SyntaxToken Current => Peek(0);

    private void Next() => _position++;

    public SqlClause NextClause()
    {
        return Current.Kind switch
        {
            SyntaxKind.SelectToken => Select(),
            SyntaxKind.EndOfFileToken => EndOfBatch(),
            _ => Unparsed(),
        };
    }

    private SqlClause Select()
    {
        var selectKeyword = Keyword(ExpressionKind.SelectKeyword);
        Next();

        var columns = new List<ColumnExpression>();

        while (Current.Kind != SyntaxKind.FromToken && Current.Kind != SyntaxKind.EndOfFileToken)
        {
            columns.Add(ParseColumn());

            if (Current.Kind == SyntaxKind.CommaToken)
                Next();
        }

        return new SelectClause(
            selectKeyword,
            new ColumnListExpression(columns));
    }

    private KeywordExpression Keyword(ExpressionKind kind, SyntaxToken? token = null)
    {
        var t = token ?? Current;
        return new KeywordExpression(kind, t.Start, t.Length);
    }

    private ColumnExpression ParseColumn()
    {
        Trim();

        var tokens = new List<SyntaxToken>();
        var start = Current.Start;

        while (!Current.IsEndOfColumnExpression())
        {
            tokens.Add(Current);
            Next();
        }

        var end = Current.Start + Current.Length;

        var trimmedTokens = tokens.Where(x => x.Kind != SyntaxKind.WhitespaceToken).ToArray();
        var trimmedTokenKinds = trimmedTokens.Select(x => x.Kind).ToArray();

        switch (trimmedTokenKinds)
        {
            case [SyntaxKind.LiteralToken]:
                {
                    var identifier = trimmedTokens[0];

                    return new ColumnIdentifierExpression(
                        new IdentifierExpression(identifier.Start, identifier.Length));
                }

            case [SyntaxKind.LiteralToken, SyntaxKind.LiteralToken]:
                {
                    var identifier = trimmedTokens[0];
                    var name = trimmedTokens[1];

                    return new ColumnAliasedIdentifierExpression(
                        new IdentifierExpression(identifier.Start, identifier.Length),
                        new IdentifierExpression(name.Start, name.Length),
                        null);
                }

            case [SyntaxKind.LiteralToken, SyntaxKind.AsToken, SyntaxKind.LiteralToken]:
                {
                    var identifier = trimmedTokens[0];
                    var asKeyword = trimmedTokens[1];
                    var name = trimmedTokens[2];

                    return new ColumnAliasedIdentifierExpression(
                        new IdentifierExpression(identifier.Start, identifier.Length),
                        new IdentifierExpression(name.Start, name.Length),
                        new KeywordExpression(ExpressionKind.AsKeyword, asKeyword.Start, asKeyword.Length));
                }

            case [SyntaxKind.QuotedTextToken, SyntaxKind.AsToken, SyntaxKind.LiteralToken]:
                {
                    var quotedText = trimmedTokens[0];
                    var asKeyword = trimmedTokens[1];
                    var name = trimmedTokens[2];

                    return new ColumnQuotedTextExpresstion(
                        new QuotedTextExpression(quotedText.Start, quotedText.Length),
                        new IdentifierExpression(name.Start, name.Length),
                        new KeywordExpression(ExpressionKind.AsKeyword, asKeyword.Start, asKeyword.Length));
                }

            case [SyntaxKind.QuotedTextToken, SyntaxKind.LiteralToken]:
                {
                    var quotedText = trimmedTokens[0];
                    var name = trimmedTokens[1];

                    return new ColumnQuotedTextExpresstion(
                        new QuotedTextExpression(quotedText.Start, quotedText.Length),
                        new IdentifierExpression(name.Start, name.Length),
                        null);
                }

            case [SyntaxKind.NumberToken, SyntaxKind.AsToken, SyntaxKind.LiteralToken]:
                {
                    var numberToken = trimmedTokens[0];
                    var asKeyword = trimmedTokens[1];
                    var name = trimmedTokens[2];

                    return new ColumnConstantNumberExpression(
                        new NumberExpression(numberToken.Start, numberToken.Length),
                        new IdentifierExpression(name.Start, name.Length),
                        new KeywordExpression(ExpressionKind.AsKeyword, asKeyword.Start, asKeyword.Length));
                }

            case [SyntaxKind.NumberToken, SyntaxKind.LiteralToken]:
                {
                    var numberToken = trimmedTokens[0];
                    var name = trimmedTokens[1];

                    return new ColumnConstantNumberExpression(
                        new NumberExpression(numberToken.Start, numberToken.Length),
                        new IdentifierExpression(name.Start, name.Length),
                        null);
                }

            default:
                return new InvalidColumnExpression(start, end - start);
        }
    }

    private EndOfBatchClause EndOfBatch()
    {
        return new EndOfBatchClause(new EndOfBatchExpression(Current.Start));
    }

    private UnparsedClause Unparsed()
    {
        var start = Current.Start;

        while (Current.Kind != SyntaxKind.EndOfFileToken)
            Next();

        return new UnparsedClause(new UnparsedExpression(start, Current.Start - start));
    }

    private void Trim()
    {
        while (Current.Kind == SyntaxKind.WhitespaceToken)
            Next();
    }
}
