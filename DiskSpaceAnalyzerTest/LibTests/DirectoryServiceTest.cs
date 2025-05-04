using DiskSpaceAnalyzerLib.Databases;
using DiskSpaceAnalyzerLib.Services;
using static DiskSpaceAnalyzerLib.Models.Category;

namespace DiskSpaceAnalyzerTest.LibTests;

public class DirectoryServiceTest
{
    [Fact]
    public async Task SimpleAnalyzeTest()
    {
        var dirs = Helper.Prepare("SimpleAnalyzeTest");
        await DirectoryService.Analyze([dirs[0]]);

        var directories = await DirectoryDatabase.GetDirectoriesAsync();
        var fileCount = await FileDatabase.GetFilesCountAsync();
        var directoryCount = directories.Count;
        var dirFilesCount = directories.Select(x => x.FileCount).Sum();

        Assert.True(fileCount == dirFilesCount);
        Assert.Equal(51, fileCount);
        Assert.Equal(19, directoryCount);
    }

    [Fact]
    public async Task SimpleAnalyzeRepeatTest()
    {
        var dirs = Helper.Prepare("SimpleAnalyzeTest", false);
        await DirectoryService.Analyze([dirs[0]]);

        var directories = await DirectoryDatabase.GetDirectoriesAsync();
        var fileCount = await FileDatabase.GetFilesCountAsync();
        var directoryCount = directories.Count;
        var dirFilesCount = directories.Select(x => x.FileCount).Sum();

        Assert.True(fileCount == dirFilesCount);
        Assert.Equal(51, fileCount);
        Assert.Equal(19, directoryCount);
    }

    [Fact]
    public async Task IgnoreAnalyzeTest()
    {
        var dirs = Helper.Prepare("IgnoreAnalyzeTest");
        await DirectoryService.Analyze([dirs[0]], [new(Path.Combine(dirs[0].FullName, "Разное"))]);

        var fileCount = await FileDatabase.GetFilesCountAsync();
        var directoryCount = await DirectoryDatabase.GetDirectoriesCountAsync();
        Assert.Equal(41, fileCount);
        Assert.Equal(17, directoryCount);
    }

    [Fact]
    public async Task IgnoreAnalyzeRepeatTest()
    {
        var dirs = Helper.Prepare("IgnoreAnalyzeTest", false);
        await DirectoryService.Analyze([dirs[0]], [new(Path.Combine(dirs[0].FullName, "Разное"))]);

        var fileCount = await FileDatabase.GetFilesCountAsync();
        var directoryCount = await DirectoryDatabase.GetDirectoriesCountAsync();
        Assert.Equal(41, fileCount);
        Assert.Equal(17, directoryCount);
    }

    [Fact]
    public async Task MultipleIgnoreAnalyzeTest()
    {
        var dirs = Helper.Prepare("MultipleIgnoreAnalyzeTest");
        var sourceDir = dirs[0].FullName;

        await DirectoryService.Analyze([dirs[0]], [
            new(Path.Combine(sourceDir, "Разное")),
            new(Path.Combine(sourceDir, "Фото разные одно имя", "1"))
        ]);

        var fileCount = await FileDatabase.GetFilesCountAsync();
        var directoryCount = await DirectoryDatabase.GetDirectoriesCountAsync();
        Assert.Equal(40, fileCount);
        Assert.Equal(15, directoryCount);
    }

    [Fact]
    public async Task GetInfoTest()
    {
        var dirs = Helper.Prepare("GetInfoTest");
        await DirectoryService.Analyze([dirs[0]]);
        var categories = await DirectoryService.GetInfo([dirs[0]]);

        var raster = categories[(int)Categories.Raster];
        Assert.Equal(15, raster.Count);
        Assert.Equal(5, raster.ClearCount);

        var vector = categories[(int)Categories.Vector];
        Assert.Equal(0, vector.Count);
        Assert.Equal(0, vector.ClearCount);

        var text = categories[(int)Categories.Text];
        Assert.Equal(7, text.Count);
        Assert.Equal(4, text.ClearCount);

        var audio = categories[(int)Categories.Audio];
        Assert.Equal(15, audio.Count);
        Assert.Equal(5, audio.ClearCount);

        var video = categories[(int)Categories.Video];
        Assert.Equal(0, video.Count);
        Assert.Equal(0, video.ClearCount);

        var eBook = categories[(int)Categories.EBook];
        Assert.Equal(0, eBook.Count);
        Assert.Equal(0, eBook.ClearCount);

        var cad = categories[(int)Categories.Cad];
        Assert.Equal(0, cad.Count);
        Assert.Equal(0, cad.ClearCount);

        var presentation = categories[(int)Categories.Presentation];
        Assert.Equal(0, presentation.Count);
        Assert.Equal(0, presentation.ClearCount);

        var spreadsheet = categories[(int)Categories.Spreadsheet];
        Assert.Equal(0, spreadsheet.Count);
        Assert.Equal(0, spreadsheet.ClearCount);

        var database = categories[(int)Categories.Database];
        Assert.Equal(0, database.Count);
        Assert.Equal(0, database.ClearCount);

        var archive = categories[(int)Categories.Archive];
        Assert.Equal(2, archive.Count);
        Assert.Equal(1, archive.ClearCount);

        var web = categories[(int)Categories.Web];
        Assert.Equal(6, web.Count);
        Assert.Equal(3, web.ClearCount);

        var developer = categories[(int)Categories.Developer];
        Assert.Equal(0, developer.Count);
        Assert.Equal(0, developer.ClearCount);

        var system = categories[(int)Categories.System];
        Assert.Equal(4, system.Count);
        Assert.Equal(2, system.ClearCount);

        var executables = categories[(int)Categories.Executables];
        Assert.Equal(0, executables.Count);
        Assert.Equal(0, executables.ClearCount);

        var settings = categories[(int)Categories.Settings];
        Assert.Equal(0, settings.Count);
        Assert.Equal(0, settings.ClearCount);

        var other = categories[(int)Categories.Other];
        Assert.Equal(2, other.Count);
        Assert.Equal(1, other.ClearCount);

        var error = categories[(int)Categories.Error];
        Assert.Equal(0, error.Count);
        Assert.Equal(0, error.ClearCount);
    }

    [Fact]
    public async Task SimpleOrganizeTest()
    {
        var dirs = Helper.Prepare("SimpleOrganizeTest");
        await DirectoryService.Analyze([dirs[0]]);
        var categoryInfos = await DirectoryService.GetInfo([dirs[0]]);
        await DirectoryService.Organize(dirs[1], categoryInfos);
    }

    [Fact]
    public async Task CategoryFilterOrganizeTest()
    {
        var dirs = Helper.Prepare("CategoryFilterOrganizeTest");
        await DirectoryService.Analyze([dirs[0]]);
        var categoryInfos = await DirectoryService.GetInfo([dirs[0]]);
        Categories[] categories = [Categories.Raster, Categories.Archive];
        categoryInfos = [.. categoryInfos.Where(c => categories.Contains(c.Category))];
        await DirectoryService.Organize(dirs[1], categoryInfos);
    }
}