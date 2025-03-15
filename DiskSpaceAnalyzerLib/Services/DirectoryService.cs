using DiskSpaceAnalyzerLib.Databases;
using DiskSpaceAnalyzerLib.Extensions;
using DiskSpaceAnalyzerLib.Models;

namespace DiskSpaceAnalyzerLib.Services;

public static class DirectoryService
{
    public static async Task Analyze(
        List<DirectoryInfo> sourceDirs, List<DirectoryInfo>? ignoreDirs = null,
        IProgress<AnalyzeProgressReport>? progress = null)
    {
        foreach (DirectoryInfo dir in sourceDirs)
        {
            if (dir.IsChildDirectoryOfAny(ignoreDirs))
                progress?.Report(new(dir, "IGNORED"));
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
                    FileInfo[] files = dir.GetFiles();
                    long filesWeight = 0;
                    foreach (FileInfo file in files)
                    {
                        filesWeight += file.Length;
                        tasks.Add(FileDatabase.AddFileAsync(new() { File = file }));
                    }
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    tasks.Add(Analyze([.. dirs], ignoreDirs, progress));
                    await Task.WhenAll(tasks);
                    await DirectoryDatabase.AddDirectoryAsync(new()
                    {
                        Directory = dir,
                        FileCount = files.Length,
                        DirectoryCount = dirs.Length,
                        FilesWeight = filesWeight,
                    });
                    progress?.Report(new(dir, "ANALYZED", ReportLevel.SUCCESS));
                }
                catch (IOException)
                {
                    progress?.Report(new(dir, "I/O ERROR", ReportLevel.ERROR));
                }
                catch (UnauthorizedAccessException)
                {
                    progress?.Report(new(dir, "ACCESS ERROR", ReportLevel.ERROR));
                }
                catch (Exception)
                {
                    progress?.Report(new(dir, "INVALID ERROR", ReportLevel.ERROR));
                }

            }
        }
    }
}
