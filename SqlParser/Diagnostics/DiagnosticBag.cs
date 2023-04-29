using System.Collections;

namespace SqlParser.Diagnostics;

public sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new();

    public void ReportSelectClauseNotFound(int start, int length) => ReportError("No SELECT Clause found.", start, length);
    public void ReportFromClauseNotFound(int start, int length) => ReportError("No FROM Clause found.", start, length);

    private void ReportError(string message, int start, int length)
    {
        _diagnostics.Add(new Diagnostic(DiagnosticSeverity.Error, message, start, length));
    }

    public IEnumerator<Diagnostic> GetEnumerator() => ((IEnumerable<Diagnostic>)_diagnostics).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_diagnostics).GetEnumerator();
}
