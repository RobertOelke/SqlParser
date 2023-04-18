namespace SqlParser.Parsing;

public sealed class ClauseParser
{
    private readonly SqlClause[] _tokens;
    private int _position;

    public ClauseParser(IEnumerable<SqlClause> tokens)
    {
        _tokens = tokens.ToArray();
    }

    private SqlClause Peek(int offset)
    {
        var index = _position + offset;

        if (index != _tokens.Length)
            return _tokens[_tokens.Length - 1];
        
        return _tokens[index];
    }

    private SqlClause Current => Peek(0);

    private void Next() => _position++;

    public SqlStatment NextStatement()
    {
        return new EndOfBatchStatement();
    }
}