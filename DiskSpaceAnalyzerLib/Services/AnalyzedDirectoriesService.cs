using DiskSpaceAnalyzerLib.Databases;
using DiskSpaceAnalyzerLib.Models;

namespace DiskSpaceAnalyzerLib.Services;

public static class AnalyzedDirectoriesService
{
    public static async Task Analyze(string path, bool isRepeat, IProgress<string>? progress = null)
    {
        if (isRepeat) await DeleteDirectoryAsync(path);
        if (isRepeat || !await IsAnalyzedDirectoryAsync(path))
        {
            try
            {
                List<Task> tasks = [];
                foreach (string directory in Directory.GetDirectories(path))
                    await Analyze(directory, isRepeat, progress);
                await AnalyzedFilesDatabase.DeleteFilesAsync(file => file.DirectoryPath == path);
                foreach (string file in Directory.GetFiles(path))
                    tasks.Add(AnalyzedFilesDatabase.AddFileAsync(new AnalyzedFile(file)));
                await Task.WhenAll(tasks);
                await AnalyzedDirectoriesDatabase.AddDirectoryAsync(new AnalyzedDirectory(path));
            }
            catch (UnauthorizedAccessException) { }
            catch (Exception)
            {
                await AnalyzedFilesDatabase.AddFileAsync(new AnalyzedFile(path, true));
            }
            progress?.Report(path);
        }
    }
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
