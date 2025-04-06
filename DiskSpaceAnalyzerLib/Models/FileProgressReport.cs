namespace DiskSpaceAnalyzerLib.Models;

public class FileProgressReport(
    AnalyzedFile file,
    string message,
    ReportLevel level = ReportLevel.Info) : ProgressReport(message, level)
{
    public AnalyzedFile File { get; } = file;
}