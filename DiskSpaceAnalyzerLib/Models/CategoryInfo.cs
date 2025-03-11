using static DiskSpaceAnalyzerLib.Models.Category;

namespace DiskSpaceAnalyzerLib.Models;

public class CategoryInfo(Categories category, int fileCount, float percentages)
{
    public Categories Category => category;
    public int FilesCount => fileCount;
    public float Percentages => percentages;
}
