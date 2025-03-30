namespace DiskSpaceAnalyzerLib.Models;

public class FileProgressReport(
    FileInfo file,
    string message,
    ReportLevel level = ReportLevel.Info) : ProgressReport(message, level)
{
    public FileInfo File { get; } = file;
}