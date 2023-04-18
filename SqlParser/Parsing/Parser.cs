using SqlParser.Tokenization;

namespace SqlParser.Parsing;

public sealed class Parser
{
    private readonly ClauseParser _clauseParser;

    public Parser(string batch)
    {
        var lexer = new Lexer(batch);

        IEnumerable<SyntaxToken> Tokens()
        {
            while (lexer.NextToken() is var t && t.Kind != SyntaxKind.EndOfFileToken)
                yield return t;
        }

        var tokenParser = new TokenParser(Tokens());

        IEnumerable<SqlClause> Clauses()
        {
            while(tokenParser.NextClause() is var clause && clause.Kind != SqlClauseKind.EndOfBatchClause)
                yield return clause;
        }

        _clauseParser = new ClauseParser(Clauses());
    }

    public Parser(IEnumerable<SyntaxToken> tokens)
    {
        var tokenParser = new TokenParser(tokens);

        IEnumerable<SqlClause> Clauses()
        {
            while(tokenParser.NextClause() is var clause && clause.Kind != SqlClauseKind.EndOfBatchClause)
                yield return clause;
        }

        _clauseParser = new ClauseParser(Clauses());
    }

    public IEnumerable<SqlStatment> Parse()
    {
        while (_clauseParser.NextStatement() is var statement && statement.Kind != StatmentKind.EndOfBatch)
        {
            yield return statement;
        }
    }
}
