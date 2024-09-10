using DiskSpaceAnalyzerLib.Databases;
using DiskSpaceAnalyzerLib.Models;
using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerLib.Services;

public class AnalyzedFilesService
{
    public static async Task<List<CategoryInfo>> GetInfo()
    {
        List<CategoryInfo> categories = [];
        int filesCount = await AnalyzedFilesDatabase.GetFilesCountAsync();
        if (filesCount == 0) return categories;
        foreach (FileTypes category in Enum.GetValues<FileTypes>())
        {
            int categoryFilesCount = await AnalyzedFilesDatabase.GetFilesCountAsync(f => f.Type == category);
            categories.Add(new(category, categoryFilesCount, (float)categoryFilesCount / filesCount));
        }
        return categories;
    }
    public static async Task<List<CategoryInfo>> GetInfo(List<string> paths)
    {
        List<CategoryInfo> categories = [];
        int filesCount = 0;
        foreach (string path in paths)
            filesCount +=
                await AnalyzedFilesDatabase.GetFilesCountAsync(f => f.DirectoryPath.StartsWith(path));
        if (filesCount == 0) return categories;
        foreach (FileTypes category in Enum.GetValues<FileTypes>())
        {
            int categoryFilesCount = 0;
            foreach (string path in paths)
                categoryFilesCount +=
                    await AnalyzedFilesDatabase.GetFilesCountAsync(
                        f => f.DirectoryPath.StartsWith(path)
                        && f.Type == category);
            categories.Add(new(category, categoryFilesCount, (float)categoryFilesCount / filesCount));
        }
        return categories;
    }
}
