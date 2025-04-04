using DiskSpaceAnalyzerLib.Databases;
using DiskSpaceAnalyzerLib.Extensions;
using DiskSpaceAnalyzerLib.Models;
using static DiskSpaceAnalyzerLib.Models.Category;

namespace DiskSpaceAnalyzerLib.Services;

public static class DirectoryService
{
    public static async Task Analyze(
        List<DirectoryInfo> sourceDirs, List<DirectoryInfo>? ignoreDirs = null,
        IProgress<DirectoryProgressReport>? directoryProgressReport = null)
    {
        foreach (var dir in sourceDirs)
        {
            var analyzedDir = new AnalyzedDirectory { Directory = dir };
            if (dir.IsChildDirectoryOfAny(ignoreDirs))
                directoryProgressReport?.Report(new(analyzedDir, "IGNORED"));
            else
            {
                List<string> directoriesToDelete = [dir.Root.FullName, dir.FullName];
                var parentDirectory = dir.Parent;
                while (parentDirectory is not null)
                {
                    directoriesToDelete.Add(parentDirectory.FullName);
                    parentDirectory = parentDirectory.Parent;
                }

                await DirectoryDatabase.DeleteDirectoriesAsync(
                    directory => directoriesToDelete.Contains(directory.DirectoryPath));
                try
                {
                    List<Task> tasks = [];
                    await FileDatabase.DeleteFilesAsync(file => file.DirectoryPath == dir.FullName);
                    FileInfo[] files = [.. dir.GetFiles().Where(file => file.Attributes != FileAttributes.Hidden)];
                    long filesWeight = 0;
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

                    var isIgnored = dir.IsParentDirectoryOfAny(ignoreDirs);
                    if (!isIgnored)
                        directoryProgressReport?.Report(new(analyzedDir, "ANALYZED", ReportLevel.Success));
                    else
                        directoryProgressReport?.Report(new(analyzedDir, "IGNORED"));

                    if (!isIgnored) await DirectoryDatabase.AddDirectoryAsync(analyzedDir);

                    await Analyze([.. dirs], ignoreDirs, directoryProgressReport);
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
        }
    }

    public static async Task<List<CategoryInfo>> GetInfo(
        List<DirectoryInfo> sourceDirs, List<DirectoryInfo>? ignoreDirs = null,
        Categories[]? categories = null,
        bool isFastCompare = true,
        bool isFileNameCompare = false
    )
    {
        List<CategoryInfo> categoryInfos = [];
        var fileCount = 0;
        long fileWeights = 0;
        foreach (var category in categories ?? Enum.GetValues<Categories>())
        {
            var files = await FileDatabase.GetFilesAsync(file => file.Category == category);
            files =
            [
                .. files.Where(file =>
                {
                    var dir = file.File.Directory!;
                    return dir.IsChildDirectoryOfAny(sourceDirs)
                           && (ignoreDirs == null
                               || !dir.IsChildDirectoryOfAny(ignoreDirs));
                })
            ];
            var duplicates = FileService.GetFileDuplicates(files, isFastCompare, isFileNameCompare);
            var categoryInfo = new CategoryInfo
            {
                Category = category,
                Files = files,
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

    public static async Task<List<CategoryInfo>?> Organize(
        DirectoryInfo outputDir,
        List<DirectoryInfo> sourceDirs, List<DirectoryInfo>? ignoreDirs = null,
        Categories[]? categories = null,
        List<CategoryInfo>? categoryInfos = null,
        bool isFastCompare = true, bool isFileNameCompare = false,
        IProgress<ProgressReport>? progressReport = null,
        IProgress<CategoryProgressReport>? categoryProgressReport = null,
        IProgress<FileProgressReport>? fileProgressReport = null)
    {
        DirectoryInfo dsaOutputDir = new(Path.Combine(outputDir.FullName, $"Organized Data {DateTime.Now}"));
        var availableFreeSpace = new DriveInfo(outputDir.Root.FullName).AvailableFreeSpace;
        categoryInfos ??= await GetInfo(sourceDirs, ignoreDirs, categories, isFastCompare, isFileNameCompare);
        var requiredWeight = categoryInfos.Sum(categoryInfo => categoryInfo.ClearWeight);
        if (requiredWeight > availableFreeSpace)
        {
            progressReport?.Report(new("NOT ENOUGH SPACE", ReportLevel.Error));
            return null;
        }

        foreach (var categoryInfo in categoryInfos)
        {
            categoryProgressReport?.Report(new(categoryInfo, "START"));
            DirectoryInfo categoryOutputDir =
                new(Path.Combine(dsaOutputDir.FullName, categoryInfo.Category.ToString()));
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
            categoryProgressReport?.Report(new(categoryInfo, "ORGANIZED", ReportLevel.Success));
        }

        return categoryInfos;
    }
}