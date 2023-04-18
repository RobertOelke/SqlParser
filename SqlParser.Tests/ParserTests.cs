using SqlParser.Parsing;

namespace SqlParser.Tests;

public class ParserTests
{
    [Fact]
    public void EmptyBatchResultsInEmptyList()
    {
        var emptyBatch = "";

        var parser = new Parser(emptyBatch);

        var parserdPatch = parser.Parse();

        Assert.Empty(parserdPatch);
    }
}
