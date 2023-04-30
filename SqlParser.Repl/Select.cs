using SqlParser.Data;
using SqlParser.Diagnostics;
using System.Collections.Immutable;

namespace SqlParser.Repl;

public static class Select
{
    public static void Handle(string sql)
    {
        var tableBuilder = ImmutableList.CreateBuilder<TableData>();

        var dataContext = new DataContext(tableBuilder.ToImmutableList());

        var compilation = Compilation.Select(sql, dataContext);
        Console.WriteLine($"└─> Formated:");
        Console.WriteLine($"  ├ {compilation.Formated}");
        var warningCount = compilation.Diagnostics.Count(d => d.Severity == Diagnostics.DiagnosticSeverity.Warning);
        if (warningCount > 0)
            Console.WriteLine($"  ├ Number of warnings: {warningCount}");

        var errorCount = compilation.Diagnostics.Count(d => d.Severity == Diagnostics.DiagnosticSeverity.Error);
        if (errorCount > 0)
            Console.WriteLine($"  ├ Number of errors:   {errorCount}");

        bool cancelRequested = false;

        while (!cancelRequested)
        {
            Console.Write("  > ");
            var line = Console.ReadLine();

            switch (line)
            {
                case "tokens":
                    foreach (var token in compilation.RawTokens)
                    {
                        Console.WriteLine($"    ├ {token}");
                    }
                    break;

                case "errors":
                    foreach (var error in compilation.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
                    {
                        PrintDiagnostic(compilation, error);
                    }
                    break;

                case "warnings":
                    foreach (var warning in compilation.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Warning))
                    {
                        PrintDiagnostic(compilation, warning);
                    }
                    break;

                case "exit":
                    cancelRequested = true;
                    break;
            }
        }
    }

    private static void PrintDiagnostic(SelectCompilation compilation, Diagnostic diagnostic)
    {
        var start = new string(' ', diagnostic.Start);
        var beforeWarning = compilation.Sql[..diagnostic.Start];
        var warningText = compilation.Sql.Substring(diagnostic.Start, diagnostic.Length);
        var afterWarning = compilation.Sql[(diagnostic.Start + diagnostic.Length)..];

        Console.Write($"    ├ {beforeWarning}");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(warningText);
        Console.ResetColor();
        Console.WriteLine(afterWarning);
        Console.WriteLine($"    │ {start}└─> {diagnostic.Message}");
    }
}
