namespace DiskSpaceAnalyzerLib.Models;

public class DirectoryProgressReport(
    DirectoryInfo dir,
    string message,
    ReportLevel level = ReportLevel.Info) : ProgressReport(message, level)
{
    public DirectoryInfo Dir { get; } = dir;
}