namespace DiskSpaceAnalyzerLib.Models;

public class DirectoryProgressReport(
    AnalyzedDirectory dir,
    string message,
    ReportLevel level = ReportLevel.Info) : ProgressReport(message, level)
{
    public AnalyzedDirectory Dir { get; } = dir;
}