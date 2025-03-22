using static DiskSpaceAnalyzerLib.Models.Category;

namespace DiskSpaceAnalyzerLib.Models;

public class CategoryInfo(Categories category, List<AnalyzedFile> files, float percentages)
{
    public Categories Category => category;
    public List<AnalyzedFile> Files => files;
    public float Percentages { get; set; } = percentages;
}
