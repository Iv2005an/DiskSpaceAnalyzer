using DiskSpaceAnalyzerLib.Databases;
using DiskSpaceAnalyzerLib.Extensions;
using DiskSpaceAnalyzerLib.Models;
using static DiskSpaceAnalyzerLib.Models.Category;

namespace DiskSpaceAnalyzerLib.Services;

public static class DirectoryService
{
    public static async Task Analyze(
        List<DirectoryInfo> sourceDirs,
        List<DirectoryInfo>? ignoreDirs = null,
        bool repeat = false,
        IProgress<DirectoryProgressReport>? directoryProgressReport = null)
    {
        await ClearDb();
        foreach (var dir in sourceDirs)
        {
            await AnalyzeDir(dir, ignoreDirs, repeat, directoryProgressReport);
        }
    }

    private static async Task AnalyzeDir(
        DirectoryInfo dir,
        List<DirectoryInfo>? ignoreDirs = null,
        bool repeat = false,
        IProgress<DirectoryProgressReport>? directoryProgressReport = null)
    {
        var analyzedDir = new AnalyzedDirectory { Directory = dir };
        if (ignoreDirs is not null && dir.IsChildDirectoryOf(ignoreDirs))
        {
            directoryProgressReport?.Report(new(analyzedDir, "IGNORED"));
            return;
        }

        if (!repeat && await DirectoryDatabase.GetDirectoriesCountAsync(
                d => d.DirectoryPath == analyzedDir.DirectoryPath) > 0)
        {
            directoryProgressReport?.Report(new(analyzedDir, "ANALYZED"));
            return;
        }

        List<Task> tasks = [];
        long filesWeight = 0;
        try
        {
            FileInfo[] files = [.. dir.GetFiles().Where(file => file.Attributes != FileAttributes.Hidden)];
            await ClearDb(dir);
            foreach (var file in files)
            {
                filesWeight += file.Length;
                tasks.Add(FileDatabase.AddFileAsync(new() { File = file }));
            }

            await Task.WhenAll(tasks);

            var dirs = dir.GetDirectories();
            analyzedDir.FileCount = files.Length;
            analyzedDir.DirectoryCount = dirs.Length;
            analyzedDir.FilesWeight = filesWeight;

            if (ignoreDirs is null || !dir.IsParentDirectoryOf(ignoreDirs))
            {
                await DirectoryDatabase.DeleteDirectoriesAsync(
                    d => d.DirectoryPath == analyzedDir.DirectoryPath);
                await DirectoryDatabase.AddDirectoryAsync(analyzedDir);
            }

            directoryProgressReport?.Report(new(analyzedDir, "ANALYZED", ReportLevel.Success));

            foreach (var d in dirs)
                await AnalyzeDir(d, ignoreDirs, repeat, directoryProgressReport);
        }
        catch (IOException)
        {
            directoryProgressReport?.Report(new(analyzedDir, "I/O ERROR", ReportLevel.Error));
        }
        catch (UnauthorizedAccessException)
        {
            directoryProgressReport?.Report(new(analyzedDir, "ACCESS ERROR", ReportLevel.Error));
        }
        catch (Exception)
        {
            directoryProgressReport?.Report(new(analyzedDir, "INVALID ERROR", ReportLevel.Error));
        }
    }

    private static async Task ClearDb()
    {
        var allDirs = await DirectoryDatabase.GetDirectoriesAsync();
        var notExistDirs = allDirs
            .Where(d => !d.Directory.Exists)
            .Select(d => d.DirectoryPath);
        await DirectoryDatabase.DeleteDirectoriesAsync(
            d => notExistDirs.Contains(d.DirectoryPath));
        await FileDatabase.DeleteFilesAsync(
            f => notExistDirs.Contains(f.DirectoryPath));
    }

    private static async Task ClearDb(DirectoryInfo dir) =>
        await FileDatabase.DeleteFilesAsync(file => file.DirectoryPath == dir.FullName);

    public static async Task<List<CategoryInfo>> GetInfo(
        List<DirectoryInfo> sourceDirs,
        List<DirectoryInfo>? ignoreDirs = null,
        bool repeat = false,
        IProgress<DirectoryProgressReport>? directoryProgressReport = null
    )
    {
        await Analyze(sourceDirs, ignoreDirs, repeat, directoryProgressReport);

        if (sourceDirs.Count == 0) return [];

        List<CategoryInfo> categoryInfos = [];
        var fileCount = 0;
        long fileWeights = 0;
        var allFiles = await FileDatabase.GetFilesAsync();
        foreach (var category in Enum.GetValues<Categories>())
        {
            var categoryFiles = allFiles.Where(file =>
            {
                var fileDir = file.File.Directory!;
                return file.Category == category
                       && fileDir.IsChildDirectoryOf(sourceDirs)
                       && (ignoreDirs is null
                           || !fileDir.IsChildDirectoryOf(ignoreDirs));
            }).ToList();
            var duplicates = FileService.GetFileDuplicates(categoryFiles);
            var categoryInfo = new CategoryInfo
            {
                Category = category,
                Files = categoryFiles,
                Duplicates = duplicates
            };
            fileCount += categoryInfo.Count;
            fileWeights += categoryInfo.Weight;
            categoryInfos.Add(categoryInfo);
        }

        foreach (var categoryInfo in categoryInfos)
            categoryInfo.CalculatePercentages(fileCount, fileWeights);
        return categoryInfos;
    }

    public static void Organize(
        DirectoryInfo outputDir,
        List<CategoryInfo> categoryInfos,
        IProgress<FileProgressReport>? fileProgressReport = null)
    {
        DirectoryInfo organizeOutputDir =
            new(Path.Combine(outputDir.FullName, $"Organized Data {DateTime.Now:yyyy-MM-dd HH.mm.ss}"));

        foreach (var categoryInfo in categoryInfos)
        {
            DirectoryInfo categoryOutputDir =
                new(Path.Combine(organizeOutputDir.FullName, categoryInfo.Category.ToString()));
            foreach (var analyzedFile in categoryInfo.Files)
                FileService.Copy(
                    analyzedFile,
                    categoryOutputDir,
                    fileProgressReport);
            foreach (var duplicate in categoryInfo.Duplicates)
                FileService.Copy(
                    duplicate[0],
                    categoryOutputDir,
                    fileProgressReport);
        }
    }

    public static bool CheckAvailableDiskSpace(DirectoryInfo dir, List<CategoryInfo> categoryInfos)
    {
        var availableFreeSpace = new DriveInfo(dir.Root.FullName).AvailableFreeSpace;
        var requiredWeight = categoryInfos.Sum(categoryInfo => categoryInfo.ClearWeight);
        return availableFreeSpace > requiredWeight;
    }

    public static bool CheckAvailableDiskSpace(DirectoryInfo dir, long requiredWeight)
    {
        var availableFreeSpace = new DriveInfo(dir.Root.FullName).AvailableFreeSpace;
        return availableFreeSpace < requiredWeight;
    }
}