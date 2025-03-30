namespace DiskSpaceAnalyzerLib.Models;

public class ProgressReport(string message, ReportLevel level = ReportLevel.Info)
{
    public string Message { get; } = message;
    public ReportLevel Level { get; } = level;
}