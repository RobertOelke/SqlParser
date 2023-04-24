using SqlParser.Parsing;
using SqlParser.Tokenization;

namespace SqlParser.Tests;

public class ParserTests
{
    private static IEnumerable<SyntaxToken> Tokens(string text)
    {
        var lexer = new Lexer(text);

        while (lexer.NextToken() is var t)
        {
            yield return t;

            if (t.Kind == SyntaxKind.EndOfFileToken)
                break;
        }
    }

    private static IEnumerable<SqlClause> Clauses(Parser parser)
    {
        while (parser.NextClause() is var c)
        {
            yield return c;

            if (c.Kind == SqlClauseKind.EndOfBatchClause)
                break;
        }
    }

    [Fact]
    public void EmptyBatchResultsInEndOfBatchClause()
    {
        var emptyBatch = "";

        var clauses = Clauses(new Parser(Tokens(emptyBatch), emptyBatch));

        Assert.Single(clauses);
        Assert.Equal(SqlClauseKind.EndOfBatchClause, clauses.First().Kind);
    }

    [Fact]
    public void ParserSimpleSelectStatement()
    {
        var selectBatch = "SELECT A, C ";

        var parser = new Parser(Tokens(selectBatch), selectBatch);

        var selectClause = Assert.IsType<SelectClause>(parser.NextClause());
        Assert.Equal(SqlClauseKind.SelectClause, selectClause.Kind);

        Assert.Equal("SELECT".Length, selectClause.SelectKeyword.Length);

        Assert.Equal(2, selectClause.ColumnList.Items.Count);
        Assert.Equal("A, C".Length, selectClause.ColumnList.Length);
        Assert.Equal("SELECT ".Length, selectClause.ColumnList.Start);

        Assert.Equal("A, C", selectBatch.Substring(selectClause.ColumnList.Start, selectClause.ColumnList.Length));

        var _ = Assert.IsType<EndOfBatchClause>(parser.NextClause());
    }

    [Fact]
    public void ParseSimpleSelectWithAliasedColumn()
    {
        var selectBatch = "SELECT A AS B, C";

        var parser = new Parser(Tokens(selectBatch), selectBatch);

        var selectClause = Assert.IsType<SelectClause>(parser.NextClause());
        Assert.Equal(SqlClauseKind.SelectClause, selectClause.Kind);
        Assert.Equal(2, selectClause.ColumnList.Items.Count);
        Assert.Equal("A AS B, C".Length, selectClause.ColumnList.Length);
        Assert.Equal("SELECT ".Length, selectClause.ColumnList.Start);

        var aliased = Assert.IsType<ColumnAliasedIdentifierExpression>(selectClause.ColumnList.Items[0]);

        Assert.Equal("A", aliased.Identifier.Text(selectBatch));
        Assert.NotNull(aliased.AsKeyword);
        Assert.Equal("AS", aliased.AsKeyword.Text(selectBatch));
        Assert.Equal("B", aliased.Alias.Text(selectBatch));

        Assert.IsType<ColumnIdentifierExpression>(selectClause.ColumnList.Items[1]);
    }
}
