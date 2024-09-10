using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerLib.Models;

public class CategoryInfo(FileTypes category, int fileCount, float percentages)
{
    public FileTypes Category => category;
    public int FilesCount => fileCount;
    public float Percentages => percentages;
}
