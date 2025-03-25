using DiskSpaceAnalyzerLib.Databases;
using DiskSpaceAnalyzerLib.Models;
using DiskSpaceAnalyzerLib.Services;
using static DiskSpaceAnalyzerLib.Models.Category;

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
        Assert.Equal(61, fileCount);
        Assert.Equal(19, directoryCount);
    }

    [Fact]
    public async Task SimpleAnalyzeRepeatTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("SimpleAnalyzeTest", false);
        await DirectoryService.Analyze([dirs[0]]);

        List<AnalyzedDirectory> directories = await DirectoryDatabase.GetDirectoriesAsync();
        int fileCount = await FileDatabase.GetFilesCountAsync();
        int directoryCount = directories.Count;
        int dirFilesCount = directories.Select(x => x.FileCount).Sum();

        Assert.True(fileCount == dirFilesCount);
        Assert.Equal(61, fileCount);
        Assert.Equal(19, directoryCount);
    }

    [Fact]
    public async Task IgnoreAnalyzeTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("IgnoreAnalyzeTest");
        await DirectoryService.Analyze([dirs[0]], [new(Path.Combine(dirs[0].FullName, "Разное"))]);

        int fileCount = await FileDatabase.GetFilesCountAsync();
        int directoryCount = await DirectoryDatabase.GetDirectoriesCountAsync();
        Assert.Equal(50, fileCount);
        Assert.Equal(17, directoryCount);
    }

    [Fact]
    public async Task IgnoreAnalyzeRepeatTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("IgnoreAnalyzeTest", false);
        await DirectoryService.Analyze([dirs[0]], [new(Path.Combine(dirs[0].FullName, "Разное"))]);

        int fileCount = await FileDatabase.GetFilesCountAsync();
        int directoryCount = await DirectoryDatabase.GetDirectoriesCountAsync();
        Assert.Equal(50, fileCount);
        Assert.Equal(17, directoryCount);
    }

    [Fact]
    public async Task MultipleIgnoreAnalyzeTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("MultipleIgnoreAnalyzeTest");
        string sourceDir = dirs[0].FullName;

        await DirectoryService.Analyze([dirs[0]], [
            new(Path.Combine(sourceDir, "Разное")),
            new(Path.Combine(sourceDir, "Фото разные одно имя", "1")),
        ]);

        int fileCount = await FileDatabase.GetFilesCountAsync();
        int directoryCount = await DirectoryDatabase.GetDirectoriesCountAsync();
        Assert.Equal(49, fileCount);
        Assert.Equal(15, directoryCount);
    }

    [Fact]
    public async Task GetInfoTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("GetInfoTest"); await DirectoryService.Analyze([dirs[0]]);
        List<CategoryInfo> categories = await DirectoryService.GetInfo([dirs[0]]);

        CategoryInfo raster = categories[(int)Categories.Raster];
        Assert.Empty(raster.Files);
        Assert.Equal(5, raster.Duplicates.Count);
        Assert.Equal((float)0.2, raster.Percentages);

        CategoryInfo vector = categories[(int)Categories.Vector];
        Assert.Empty(vector.Files);
        Assert.Empty(vector.Duplicates);
        Assert.Equal(0, vector.Percentages);

        CategoryInfo text = categories[(int)Categories.Text];
        Assert.Single(text.Files);
        Assert.Equal(3, text.Duplicates.Count);
        Assert.Equal((float)0.16, text.Percentages);

        CategoryInfo audio = categories[(int)Categories.Audio];
        Assert.Empty(audio.Files);
        Assert.Equal(5, audio.Duplicates.Count);
        Assert.Equal((float)0.2, audio.Percentages);

        CategoryInfo video = categories[(int)Categories.Video];
        Assert.Empty(video.Files);
        Assert.Empty(video.Duplicates);
        Assert.Equal(0, video.Percentages);

        CategoryInfo eBook = categories[(int)Categories.EBook];
        Assert.Empty(eBook.Files);
        Assert.Empty(eBook.Duplicates);
        Assert.Equal(0, eBook.Percentages);

        CategoryInfo cad = categories[(int)Categories.CAD];
        Assert.Empty(cad.Files);
        Assert.Empty(cad.Duplicates);
        Assert.Equal(0, cad.Percentages);

        CategoryInfo presentation = categories[(int)Categories.Presentation];
        Assert.Empty(presentation.Files);
        Assert.Empty(presentation.Duplicates);
        Assert.Equal(0, presentation.Percentages);

        CategoryInfo spreadsheet = categories[(int)Categories.Spreadsheet];
        Assert.Empty(spreadsheet.Files);
        Assert.Empty(spreadsheet.Duplicates);
        Assert.Equal(0, spreadsheet.Percentages);

        CategoryInfo database = categories[(int)Categories.Database];
        Assert.Empty(database.Files);
        Assert.Empty(database.Duplicates);
        Assert.Equal(0, database.Percentages);

        CategoryInfo archive = categories[(int)Categories.Archive];
        Assert.Empty(archive.Files);
        Assert.Single(archive.Duplicates);
        Assert.Equal((float)0.04, archive.Percentages);

        CategoryInfo web = categories[(int)Categories.Web];
        Assert.Empty(web.Files);
        Assert.Equal(3, web.Duplicates.Count);
        Assert.Equal((float)0.12, web.Percentages);

        CategoryInfo developer = categories[(int)Categories.Developer];
        Assert.Empty(developer.Files);
        Assert.Empty(developer.Duplicates);
        Assert.Equal(0, developer.Percentages);

        CategoryInfo system = categories[(int)Categories.System];
        Assert.Empty(system.Files);
        Assert.Equal(2, system.Duplicates.Count);
        Assert.Equal((float)0.08, system.Percentages);

        CategoryInfo executables = categories[(int)Categories.Executables];
        Assert.Empty(executables.Files);
        Assert.Empty(executables.Duplicates);
        Assert.Equal(0, executables.Percentages);

        CategoryInfo settings = categories[(int)Categories.Settings];
        Assert.Empty(settings.Files);
        Assert.Empty(settings.Duplicates);
        Assert.Equal(0, settings.Percentages);

        CategoryInfo other = categories[(int)Categories.Other];
        Assert.Equal(3, other.Files.Count);
        Assert.Equal(2, other.Duplicates.Count);
        Assert.Equal((float)0.2, other.Percentages);

        CategoryInfo error = categories[(int)Categories.Error];
        Assert.Empty(error.Files);
        Assert.Empty(error.Duplicates);
        Assert.Equal(0, error.Percentages);
    }

    [Fact]
    public async Task WithFileNameCompareGetInfoTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("WithFileNameCompareGetInfoTest");

        await DirectoryService.Analyze([dirs[0]]);
        List<CategoryInfo> categories = await DirectoryService.GetInfo([dirs[0]], isFileNameCompare: true);

        CategoryInfo raster = categories[(int)Categories.Raster];
        Assert.Equal(12, raster.Files.Count);
        Assert.Single(raster.Duplicates);
        Assert.Equal((float)0.3171, raster.Percentages);

        CategoryInfo vector = categories[(int)Categories.Vector];
        Assert.Empty(vector.Files);
        Assert.Empty(vector.Duplicates);
        Assert.Equal(0, vector.Percentages);

        CategoryInfo text = categories[(int)Categories.Text];
        Assert.Single(text.Files);
        Assert.Equal(3, text.Duplicates.Count);
        Assert.Equal((float)0.0976, text.Percentages);

        CategoryInfo audio = categories[(int)Categories.Audio];
        Assert.Equal(12, audio.Files.Count);
        Assert.Single(audio.Duplicates);
        Assert.Equal((float)0.3171, audio.Percentages);

        CategoryInfo video = categories[(int)Categories.Video];
        Assert.Empty(video.Files);
        Assert.Empty(video.Duplicates);
        Assert.Equal(0, video.Percentages);

        CategoryInfo eBook = categories[(int)Categories.EBook];
        Assert.Empty(eBook.Files);
        Assert.Empty(eBook.Duplicates);
        Assert.Equal(0, eBook.Percentages);

        CategoryInfo cad = categories[(int)Categories.CAD];
        Assert.Empty(cad.Files);
        Assert.Empty(cad.Duplicates);
        Assert.Equal(0, cad.Percentages);

        CategoryInfo presentation = categories[(int)Categories.Presentation];
        Assert.Empty(presentation.Files);
        Assert.Empty(presentation.Duplicates);
        Assert.Equal(0, presentation.Percentages);

        CategoryInfo spreadsheet = categories[(int)Categories.Spreadsheet];
        Assert.Empty(spreadsheet.Files);
        Assert.Empty(spreadsheet.Duplicates);
        Assert.Equal(0, spreadsheet.Percentages);

        CategoryInfo database = categories[(int)Categories.Database];
        Assert.Empty(database.Files);
        Assert.Empty(database.Duplicates);
        Assert.Equal(0, database.Percentages);

        CategoryInfo archive = categories[(int)Categories.Archive];
        Assert.Empty(archive.Files);
        Assert.Single(archive.Duplicates);
        Assert.Equal((float)0.0244, archive.Percentages);

        CategoryInfo web = categories[(int)Categories.Web];
        Assert.Empty(web.Files);
        Assert.Equal(3, web.Duplicates.Count);
        Assert.Equal((float)0.0732, web.Percentages);

        CategoryInfo developer = categories[(int)Categories.Developer];
        Assert.Empty(developer.Files);
        Assert.Empty(developer.Duplicates);
        Assert.Equal(0, developer.Percentages);

        CategoryInfo system = categories[(int)Categories.System];
        Assert.Empty(system.Files);
        Assert.Equal(2, system.Duplicates.Count);
        Assert.Equal((float)0.0488, system.Percentages);

        CategoryInfo executables = categories[(int)Categories.Executables];
        Assert.Empty(executables.Files);
        Assert.Empty(executables.Duplicates);
        Assert.Equal(0, executables.Percentages);

        CategoryInfo settings = categories[(int)Categories.Settings];
        Assert.Empty(settings.Files);
        Assert.Empty(settings.Duplicates);
        Assert.Equal(0, settings.Percentages);

        CategoryInfo other = categories[(int)Categories.Other];
        Assert.Equal(3, other.Files.Count);
        Assert.Equal(2, other.Duplicates.Count);
        Assert.Equal((float)0.122, other.Percentages);

        CategoryInfo error = categories[(int)Categories.Error];
        Assert.Empty(error.Files);
        Assert.Empty(error.Duplicates);
        Assert.Equal(0, error.Percentages);
    }

    [Fact]
    public async Task SimpleOrganizeTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("SimpleOrganizeTest");
        await DirectoryService.Analyze([dirs[0]]);
        await DirectoryService.Organize(dirs[1], [dirs[0]]);
    }

    [Fact]
    public async Task WithFileNameCompareOrganizeTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("WithFileNameCompareOrganizeTest");
        await DirectoryService.Analyze([dirs[0]]);
        await DirectoryService.Organize(dirs[1], [dirs[0]], isFileNameCompare: true);
    }

    [Fact]
    public async Task CategoryFilterOrganizeTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("CategoryFilterOrganizeTest");
        await DirectoryService.Analyze([dirs[0]]);
        await DirectoryService.Organize(dirs[1], [dirs[0]], categories:[Categories.Raster, Categories.Archive]);
    }
}