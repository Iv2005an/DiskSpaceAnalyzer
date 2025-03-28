namespace DiskSpaceAnalyzerLib.Models;

public class ProgressReport(string message, ReportLevel level = ReportLevel.INFO)
{
    public string Status { get; } = message;
    public ReportLevel Level { get; } = level;
}