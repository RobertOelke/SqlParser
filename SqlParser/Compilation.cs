using SqlParser.Binding;
using SqlParser.Data;
using SqlParser.Diagnostics;
using SqlParser.Parsing;
using SqlParser.Tokenization;

namespace SqlParser;

public sealed record SelectCompilation(
    string Sql,
    string Formated,
    IEnumerable<Diagnostic> Diagnostics,
    List<SyntaxToken> RawTokens,
    BoundSelectStatement BoundSelect);

public sealed class Compilation
{
    public static SelectCompilation Select(string select, DataContext context)
    {
        var lexer = new Lexer(select);
        var allTokens = GetTokensFromLexer(lexer).ToList();
        var parser = new Parser(allTokens);
        var binder = new Binder(context, parser, select);

        var boundSelect = binder.BindSelect();

        return new SelectCompilation(
            select,
            TokenPrinter.Print(select, allTokens),
            binder.Diagnostics,
            allTokens,
            boundSelect);
    }

    private static IEnumerable<SyntaxToken> GetTokensFromLexer(Lexer lexer)
    {
        while (lexer.NextToken() is var t)
        {
            yield return t;

            if (t.Kind == SyntaxKind.EndOfFileToken)
                yield break;
        }
    }
}
