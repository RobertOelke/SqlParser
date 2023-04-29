using SqlParser.Data;
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

                case "exit":
                    cancelRequested = true;
                    break;
            }
        }
    }
}
