namespace DiskSpaceAnalyzerLib.Models;

public class DirectoryProgressReport(
    DirectoryInfo dir,
    string message, ReportLevel level = ReportLevel.INFO) : ProgressReport(message, level)
{
    public DirectoryInfo Dir { get; } = dir;
}