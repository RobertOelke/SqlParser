using System.Collections;

namespace SqlParser.Diagnostics;

public sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new();

    public void ReportSelectClauseNotFound(int start, int length) => ReportError("No SELECT Clause found.", start, length);
    public void ReportFromClauseNotFound(int start, int length) => ReportError("No FROM Clause found.", start, length);

    public void ReportColumnNotBound(int start, int length) => ReportWarning("Column could not be bound.", start, length);
    public void ReportTableNotBound(int start, int length) => ReportWarning("Table could not be bound.", start, length);

    private void ReportError(string message, int start, int length)
    {
        _diagnostics.Add(new Diagnostic(DiagnosticSeverity.Error, message, start, length));
    }

    private void ReportWarning(string message, int start, int length)
    {
        _diagnostics.Add(new Diagnostic(DiagnosticSeverity.Warning, message, start, length));
    }

    public IEnumerator<Diagnostic> GetEnumerator() => ((IEnumerable<Diagnostic>)_diagnostics).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_diagnostics).GetEnumerator();
}
