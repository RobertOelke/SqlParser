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


        var trimmedTokenKinds = tokens.Select(x => x.Kind).Where(x => x != SyntaxKind.WhitespaceToken).ToList();

        switch (trimmedTokenKinds)
        {
            case [SyntaxKind.LiteralToken]:
                {
                    var identifier = tokens.First(x => x.Kind == SyntaxKind.LiteralToken);
                    return new ColumnIdentifierExpression(new IdentifierExpression(identifier.Start, identifier.Length));
                }

            case [SyntaxKind.LiteralToken, SyntaxKind.LiteralToken]:
                {
                    var literalTokens = tokens.Where(x => x.Kind == SyntaxKind.LiteralToken).ToList();

                    var identifier = literalTokens[0];
                    var name = literalTokens[1];

                    return new ColumnAliasedIdentifierExpression(
                        new IdentifierExpression(identifier.Start, identifier.Length),
                        new IdentifierExpression(name.Start, name.Length),
                        null);
                }

            case [SyntaxKind.LiteralToken, SyntaxKind.LiteralToken, SyntaxKind.LiteralToken]:
                {
                    var literalTokens = tokens.Where(x => x.Kind == SyntaxKind.LiteralToken).ToList();

                    var identifier = literalTokens[0];
                    var asKeyword = literalTokens[1];
                    var name = literalTokens[2];

                    if (asKeyword.Text(_src).ToUpper() != "AS")
                    {
                        return new InvalidColumnExpression(start, end - start);
                    }

                    return new ColumnAliasedIdentifierExpression(
                        new IdentifierExpression(identifier.Start, identifier.Length),
                        new IdentifierExpression(name.Start, name.Length),
                        new KeywordExpression(ExpressionKind.AsKeyword, asKeyword.Start, asKeyword.Length));
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
