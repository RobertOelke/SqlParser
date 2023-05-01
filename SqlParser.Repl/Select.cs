using SqlParser.Binding;
using SqlParser.Data;
using SqlParser.Diagnostics;
using System.Collections.Immutable;

namespace SqlParser.Repl;

public static class Select
{
    public static void Handle(string sql)
    {
        var compilation = Compilation.Select(sql, DefaultContext);
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
            if (line == null)
                continue;

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

                default:
                    if (line.StartsWith("?"))
                    {
                        var what = line[1..];

                        var matchingColumn = compilation.BoundSelect.Selection.FirstOrDefault(s => string.Equals(s.Name, what, StringComparison.InvariantCultureIgnoreCase));

                        if (matchingColumn?.BoundData is ColumnData data)
                        {
                            Console.WriteLine($"    ├ {data.Name} {data.DataType}");
                        }
                    }

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

    private static DataContext DefaultContext
    {
        get
        {
            var tableBuilder = ImmutableList.CreateBuilder<TableData>();

            tableBuilder.Add(
                new TableData(
                    "DUAL",
                    ImmutableList.Create(
                        new ColumnData(
                            1, "DUMMY", ColumnType.Varchar2, 1, null, null, 1, true 
                        )))
            );

            var dataContext = new DataContext(tableBuilder.ToImmutableList());

            return dataContext;
        }
    }
}
