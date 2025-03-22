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
        Assert.Equal(800, fileCount);
        Assert.Equal(16, directoryCount);
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
        Assert.Equal(800, fileCount);
        Assert.Equal(16, directoryCount);
    }

    [Fact]
    public async Task IgnoreAnalyzeTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("IgnoreAnalyzeTest");
        await DirectoryService.Analyze([dirs[0]], [new(Path.Combine(dirs[0].FullName, "Фото 1", "Фото 1", "Фото 1"))]);

        int fileCount = await FileDatabase.GetFilesCountAsync();
        int directoryCount = await DirectoryDatabase.GetDirectoriesCountAsync();
        Assert.Equal(699, fileCount);
        Assert.Equal(12, directoryCount);
    }

    [Fact]
    public async Task IgnoreAnalyzeRepeatTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("IgnoreAnalyzeTest", false);
        await DirectoryService.Analyze([dirs[0]], [new(Path.Combine(dirs[0].FullName, "Фото 1", "Фото 1", "Фото 1"))]);

        int fileCount = await FileDatabase.GetFilesCountAsync();
        int directoryCount = await DirectoryDatabase.GetDirectoriesCountAsync();
        Assert.Equal(699, fileCount);
        Assert.Equal(12, directoryCount);
    }

    [Fact]
    public async Task MultipleIgnoreAnalyzeTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("MultipleIgnoreAnalyzeTest");
        string sourceDir = dirs[0].FullName;

        await DirectoryService.Analyze([dirs[0]], [
            new(Path.Combine(sourceDir, "Фото 1", "Фото 1", "Фото 1")),
            new(Path.Combine(sourceDir, "Фото 2")),
        ]);

        int fileCount = await FileDatabase.GetFilesCountAsync();
        int directoryCount = await DirectoryDatabase.GetDirectoriesCountAsync();
        Assert.Equal(598, fileCount);
        Assert.Equal(11, directoryCount);
    }

    [Fact]
    public async Task GetInfoTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("GetInfoTest");

        await DirectoryService.Analyze([dirs[0]]);
        List<CategoryInfo> categories = await DirectoryService.GetInfo([dirs[0]]);

        CategoryInfo raster = categories[(int)Categories.Raster];
        Assert.Equal(712, raster.Files.Count);
        Assert.Equal((float)0.89, raster.Percentages);

        CategoryInfo vector = categories[(int)Categories.Vector];
        Assert.Empty(vector.Files);
        Assert.Equal(0, vector.Percentages);

        CategoryInfo text = categories[(int)Categories.Text];
        Assert.Equal(18, text.Files.Count);
        Assert.Equal((float)0.0225, text.Percentages);

        CategoryInfo audio = categories[(int)Categories.Audio];
        Assert.Equal(28, audio.Files.Count);
        Assert.Equal((float)0.035, audio.Percentages);

        CategoryInfo video = categories[(int)Categories.Video];
        Assert.Empty(video.Files);
        Assert.Equal(0, video.Percentages);

        CategoryInfo eBook = categories[(int)Categories.EBook];
        Assert.Empty(eBook.Files);
        Assert.Equal(0, eBook.Percentages);

        CategoryInfo cad = categories[(int)Categories.CAD];
        Assert.Empty(cad.Files);
        Assert.Equal(0, cad.Percentages);

        CategoryInfo presentation = categories[(int)Categories.Presentation];
        Assert.Empty(presentation.Files);
        Assert.Equal(0, presentation.Percentages);

        CategoryInfo spreadsheet = categories[(int)Categories.Spreadsheet];
        Assert.Empty(spreadsheet.Files);
        Assert.Equal(0, spreadsheet.Percentages);

        CategoryInfo database = categories[(int)Categories.Database];
        Assert.Empty(database.Files);
        Assert.Equal(0, database.Percentages);

        CategoryInfo archive = categories[(int)Categories.Archive];
        Assert.Equal(8, archive.Files.Count);
        Assert.Equal((float)0.01, archive.Percentages);

        CategoryInfo web = categories[(int)Categories.Web];
        Assert.Equal(12, web.Files.Count);
        Assert.Equal((float)0.015, web.Percentages);

        CategoryInfo developer = categories[(int)Categories.Developer];
        Assert.Empty(developer.Files);
        Assert.Equal(0, developer.Percentages);

        CategoryInfo system = categories[(int)Categories.System];
        Assert.Equal(3, system.Files.Count);
        Assert.Equal((float)0.00375, system.Percentages);

        CategoryInfo executables = categories[(int)Categories.Video];
        Assert.Empty(executables.Files);
        Assert.Equal(0, executables.Percentages);

        CategoryInfo settings = categories[(int)Categories.Settings];
        Assert.Equal(3, settings.Files.Count);
        Assert.Equal((float)0.00375, settings.Percentages);

        CategoryInfo other = categories[(int)Categories.Other];
        Assert.Equal(16, other.Files.Count);
        Assert.Equal((float)0.02, other.Percentages);

        CategoryInfo error = categories[(int)Categories.Error];
        Assert.Empty(error.Files);
        Assert.Equal(0, error.Percentages);
    }

    [Fact]
    public async Task MultipleIgnoreGetInfoTest()
    {
        DirectoryInfo[] dirs = Helper.Prepare("MultipleIgnoreGetInfoTest");
        string sourceDir = dirs[0].FullName;

        await DirectoryService.Analyze([dirs[0]]);
        List<CategoryInfo> categories = await DirectoryService.GetInfo([dirs[0]], [
            new(Path.Combine(sourceDir, "Фото 1", "Фото 1", "Фото 1")),
            new(Path.Combine(sourceDir, "Фото 2")),
        ]);

        CategoryInfo raster = categories[(int)Categories.Raster];
        Assert.Equal(512, raster.Files.Count);
        Assert.Equal((float)0.8562, raster.Percentages);

        CategoryInfo vector = categories[(int)Categories.Vector];
        Assert.Empty(vector.Files);
        Assert.Equal(0, vector.Percentages);

        CategoryInfo text = categories[(int)Categories.Text];
        Assert.Equal(18, text.Files.Count);
        Assert.Equal((float)0.0301, text.Percentages);

        CategoryInfo audio = categories[(int)Categories.Audio];
        Assert.Equal(28, audio.Files.Count);
        Assert.Equal((float)0.0468, audio.Percentages);

        CategoryInfo video = categories[(int)Categories.Video];
        Assert.Empty(video.Files);
        Assert.Equal(0, video.Percentages);

        CategoryInfo eBook = categories[(int)Categories.EBook];
        Assert.Empty(eBook.Files);
        Assert.Equal(0, eBook.Percentages);

        CategoryInfo cad = categories[(int)Categories.CAD];
        Assert.Empty(cad.Files);
        Assert.Equal(0, cad.Percentages);

        CategoryInfo presentation = categories[(int)Categories.Presentation];
        Assert.Empty(presentation.Files);
        Assert.Equal(0, presentation.Percentages);

        CategoryInfo spreadsheet = categories[(int)Categories.Spreadsheet];
        Assert.Empty(spreadsheet.Files);
        Assert.Equal(0, spreadsheet.Percentages);

        CategoryInfo database = categories[(int)Categories.Database];
        Assert.Empty(database.Files);
        Assert.Equal(0, database.Percentages);

        CategoryInfo archive = categories[(int)Categories.Archive];
        Assert.Equal(8, archive.Files.Count);
        Assert.Equal((float)0.0134, archive.Percentages);

        CategoryInfo web = categories[(int)Categories.Web];
        Assert.Equal(12, web.Files.Count);
        Assert.Equal((float)0.0201, web.Percentages);

        CategoryInfo developer = categories[(int)Categories.Developer];
        Assert.Empty(developer.Files);
        Assert.Equal(0, developer.Percentages);

        CategoryInfo system = categories[(int)Categories.System];
        Assert.Equal(3, system.Files.Count);
        Assert.Equal((float)0.005, system.Percentages);

        CategoryInfo executables = categories[(int)Categories.Video];
        Assert.Empty(executables.Files);
        Assert.Equal(0, executables.Percentages);

        CategoryInfo settings = categories[(int)Categories.Settings];
        Assert.Equal(3, settings.Files.Count);
        Assert.Equal((float)0.005, settings.Percentages);

        CategoryInfo other = categories[(int)Categories.Other];
        Assert.Equal(14, other.Files.Count);
        Assert.Equal((float)0.0234, other.Percentages);

        CategoryInfo error = categories[(int)Categories.Error];
        Assert.Empty(error.Files);
        Assert.Equal(0, error.Percentages);
    }
}