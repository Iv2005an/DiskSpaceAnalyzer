using DiskSpaceAnalyzerLib.Databases;
using DiskSpaceAnalyzerLib.Models;
using DiskSpaceAnalyzerLib.Services;

namespace DiskSpaceAnalyzerTest.LibTests;

public class DirectoryServiceTest
{
    [Fact]
    public async Task SimpleAnalyzeTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("SimpleAnalyzeTest");
        await DirectoryService.Analyze([dirs[0]]);

        List<AnalyzedDirectory> directories = await DirectoryDatabase.GetDirectoriesAsync();
        int fileCount = await FileDatabase.GetFilesCountAsync();
        int directoryCount = directories.Count;
        int dirFilesCount = directories.Select(x => x.FileCount).Sum();

        Assert.True(fileCount == dirFilesCount);
        Assert.Equal(700, fileCount);
        Assert.Equal(15, directoryCount);
    }

    [Fact]
    public async Task IgnoreAnalyzeTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("IgnoreAnalyzeTest");
        await DirectoryService.Analyze([dirs[0]], [new(Path.Combine(dirs[0].FullName, "Фото 1"))]);

        int fileCount = await FileDatabase.GetFilesCountAsync();
        int directoryCount = await DirectoryDatabase.GetDirectoriesCountAsync();
        Assert.Equal(296, fileCount);
        Assert.Equal(10, directoryCount);
    }

    [Fact]
    public async Task MultipleIgnoreAnalyzeTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("MultipleIgnoreAnalyzeTest");
        string sourceDir = dirs[0].FullName;
        await DirectoryService.Analyze([dirs[0]], [
            new(Path.Combine(sourceDir, "Фото 1")),
            new(Path.Combine(sourceDir, "Фото 2")),
        ]);

        int fileCount = await FileDatabase.GetFilesCountAsync();
        int directoryCount = await DirectoryDatabase.GetDirectoriesCountAsync();
        Assert.Equal(195, fileCount);
        Assert.Equal(9, directoryCount);
    }
}