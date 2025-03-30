namespace DiskSpaceAnalyzerLib.Models;

public class CategoryProgressReport(
    CategoryInfo category,
    string message,
    ReportLevel level = ReportLevel.Info) : ProgressReport(message, level)
{
    public CategoryInfo Category => category;
}