using DiskSpaceAnalyzerLib.Databases;

namespace DiskSpaceAnalyzerLib.Services;

public static class AnalyzedDirectoriesService
{
    public static async Task<List<string>> GetDirectoriesPaths() =>
       (await AnalyzedDirectoriesDatabase.GetDirectoriesAsync()).Select(
           directory => directory.DirectoryPath).ToList();
    public static async Task<bool> IsAnalyzedDirectoryAsync(string path) =>
        await AnalyzedDirectoriesDatabase.GetDirectoriesCountAsync(
            directory => directory.DirectoryPath == path) != 0;
    public static async Task DeleteDirectoryAsync(string directoryPath)
    {
        List<string> directoriesToDelete = [Directory.GetDirectoryRoot(directoryPath), directoryPath];
        DirectoryInfo? parentDirectory = Directory.GetParent(directoryPath);
        while (parentDirectory is not null)
        {
            directoriesToDelete.Add(parentDirectory.FullName);
            parentDirectory = parentDirectory.Parent;
        }
        await AnalyzedDirectoriesDatabase.DeleteDirectoryAsync(
            directory => directoriesToDelete.Contains(directory.DirectoryPath));
    }
}
