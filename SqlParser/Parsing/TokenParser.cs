using SqlParser.Tokenization;

namespace SqlParser.Parsing;

public sealed class TokenParser
{
    private readonly SyntaxToken[] _tokens;
    private int _position;
    
    public TokenParser(IEnumerable<SyntaxToken> tokens)
    {
        _tokens = tokens.ToArray();
    }

    private SyntaxToken Peek(int offset)
    {
        var index = _position + offset;

        if (index != _tokens.Length)
            return _tokens[_tokens.Length - 1];
        
        return _tokens[index];
    }

    private SyntaxToken Current => Peek(0);

    private void NextToken() => _position++;

    public SqlClause NextClause()
    {
        return new EndOfBatchClause();
    }
}
