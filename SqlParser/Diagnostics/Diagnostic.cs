namespace SqlParser.Diagnostics;

public class Diagnostic
{
    public Diagnostic(DiagnosticSeverity severity, string message, int start, int length)
    {
        Severity = severity;
        Message = message;
        Start = start;
        Length = length;
    }

    public DiagnosticSeverity Severity { get; }
    public string Message { get; }
    public int Start { get; }
    public int Length { get; }
}
