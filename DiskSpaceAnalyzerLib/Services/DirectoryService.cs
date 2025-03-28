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
        foreach (DirectoryInfo dir in sourceDirs)
        {
            if (dir.IsChildDirectoryOfAny(ignoreDirs)) directoryProgressReport?.Report(new(dir, "IGNORED"));
            else
            {
                List<string> directoriesToDelete = [dir.Root.FullName, dir.FullName];
                DirectoryInfo? parentDirectory = dir.Parent;
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
                    FileInfo[] files = [.. dir.GetFiles().Where(file => file.Extension != ".DS_Store")];
                    long filesWeight = 0;
                    foreach (FileInfo file in files)
                    {
                        filesWeight += file.Length;
                        tasks.Add(FileDatabase.AddFileAsync(new() { File = file }));
                    }
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    tasks.Add(Analyze([.. dirs], ignoreDirs, directoryProgressReport));
                    await Task.WhenAll(tasks);
                    if (ignoreDirs is null || !dir.IsParentDirectoryOfAny(ignoreDirs))
                        await DirectoryDatabase.AddDirectoryAsync(new()
                        {
                            Directory = dir,
                            FileCount = files.Length,
                            DirectoryCount = dirs.Length,
                            FilesWeight = filesWeight,
                        });
                    directoryProgressReport?.Report(new(dir, "ANALYZED", ReportLevel.SUCCESS));
                }
                catch (IOException) { directoryProgressReport?.Report(new(dir, "I/O ERROR", ReportLevel.ERROR)); }
                catch (UnauthorizedAccessException) { directoryProgressReport?.Report(new(dir, "ACCESS ERROR", ReportLevel.ERROR)); }
                catch (Exception) { directoryProgressReport?.Report(new(dir, "INVALID ERROR", ReportLevel.ERROR)); }
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
        int fileCount = 0;
        long fileWeights = 0;
        foreach (Categories category in categories ?? Enum.GetValues<Categories>())
        {
            List<AnalyzedFile> files = await FileDatabase.GetFilesAsync(file => file.Category == category);
            files = [.. files.Where(file => {
                DirectoryInfo dir = file.File.Directory!;
                return dir.IsChildDirectoryOfAny(sourceDirs)
                    && (ignoreDirs == null
                        || !dir.IsChildDirectoryOfAny(ignoreDirs));
            })];
            List<List<AnalyzedFile>> duplicates = FileService.GetFileDuplicates(files, isFastCompare, isFileNameCompare);
            fileCount += files.Count + duplicates.Count;
            long categoryWeight = files.Sum(file => file.Weight) + duplicates.Sum(list => list.Sum(file => file.Weight));
            fileWeights += categoryWeight;
            long categoryClearWeight = files.Sum(file => file.Weight) + duplicates.Sum(list => list[0].Weight);
            categoryInfos.Add(new(category, 0, 0, categoryWeight, categoryClearWeight, files, duplicates));
        }
        return [.. categoryInfos.Select(categoryInfo =>
        {
            categoryInfo.CountPercentages = (float)Math.Round(
                (float)(categoryInfo.Files.Count + categoryInfo.Duplicates.Count) / fileCount, 4);
            categoryInfo.WeightPercentages = (float)Math.Round(
                (float)categoryInfo.Weight / fileWeights, 4);
            return categoryInfo;
        })];
    }

    public static async Task<List<CategoryInfo>?> Organize(
        DirectoryInfo outputDir,
        List<DirectoryInfo> sourceDirs, List<DirectoryInfo>? ignoreDirs = null,
        Categories[]? categories = null,
        bool isFastCompare = true, bool isFileNameCompare = false,
        IProgress<ProgressReport>? progressReport = null,
        IProgress<CategoryProgressReport>? categoryProgressReport = null,
        IProgress<FileProgressReport>? fileProcessReport = null)
    {
        DirectoryInfo dsaOutputDir = new(Path.Combine(outputDir.FullName, $"Organized Data {DateTime.Now}"));
        long availableFreeSpace = new DriveInfo(outputDir.Root.FullName).AvailableFreeSpace;
        List<CategoryInfo> categoryInfos = await GetInfo(sourceDirs, ignoreDirs, categories, isFastCompare, isFileNameCompare);
        long requiredWeight = categoryInfos.Sum(categoryInfo => categoryInfo.ClearWeight);
        if (requiredWeight > availableFreeSpace)
        {
            progressReport?.Report(new("NOT ENOUGH SPACE", ReportLevel.ERROR));
            return null;
        }
        foreach (CategoryInfo categoryInfo in categoryInfos)
        {
            categoryProgressReport?.Report(new(categoryInfo, "START"));
            DirectoryInfo categoryOutputDir = new(Path.Combine(dsaOutputDir.FullName, categoryInfo.Category.ToString()));
            foreach (AnalyzedFile analyzedFile in categoryInfo.Files)
                FileService.Copy(
                    analyzedFile,
                    categoryOutputDir,
                    fileProcessReport);
            foreach (List<AnalyzedFile> duplicate in categoryInfo.Duplicates)
                FileService.Copy(
                    duplicate[0],
                    categoryOutputDir,
                    fileProcessReport);
            categoryProgressReport?.Report(new(categoryInfo, "ORGANIZED", ReportLevel.SUCCESS));
        }
        return categoryInfos;
    }
}
