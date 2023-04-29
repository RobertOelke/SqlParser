using System.Collections;

namespace SqlParser.Diagnostics;

public sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new();

    public IEnumerator<Diagnostic> GetEnumerator() => ((IEnumerable<Diagnostic>)_diagnostics).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_diagnostics).GetEnumerator();

    public void ReportError(string message, int start, int length)
    {
        _diagnostics.Add(new Diagnostic(DiagnosticSeverity.Error, message, start, length));
    }
}
