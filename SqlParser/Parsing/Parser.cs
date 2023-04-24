using SqlParser.Tokenization;

namespace SqlParser.Parsing;

public sealed class Parser
{
    private readonly SyntaxToken[] _tokens;
    private int _position;
    
    public Parser(IEnumerable<SyntaxToken> tokens)
    {
        _tokens = tokens.ToArray();
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
        try
        {
            Trim();
            var start = Current.Start;

            switch (Current.Kind)
            {
                case SyntaxKind.LiteralToken:
                    var identifier = new IdentifierExpression(Current.Start, Current.Length);

                    Next();

                    return new ColumnIdentifierExpression(identifier);

                default:
                    while (!Current.IsColumnSeparator())
                        Next();

                    var end = Current.Start + Current.Length;

                    return new InvalidColumnExpression(start, end - start);
            }
        }
        finally
        {
            Trim();

            if (Current.IsColumnSeparator())
                Next();
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
