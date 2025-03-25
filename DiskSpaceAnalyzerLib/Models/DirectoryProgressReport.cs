namespace DiskSpaceAnalyzerLib.Models;

public class DirectoryProgressReport(DirectoryInfo dir, string message, ReportLevel level = ReportLevel.INFO)
{
    public DirectoryInfo Dir { get; } = dir;
    public string Status { get; } = message;
    public ReportLevel Level { get; } = level;
}