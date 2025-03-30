using DiskSpaceAnalyzerLib.Databases;

namespace DiskSpaceAnalyzerTest.LibTests;

public class FileDatabaseTest
{
    [Fact]
    public async Task AddFileTest()
    {
        Helper.Prepare("AddFileTest");
        await FileDatabase.AddFileAsync(new());
        Assert.Equal(1, await FileDatabase.GetFilesCountAsync());
    }

    [Fact]
    public async Task DeleteFilesTest()
    {
        Helper.Prepare("DeleteFilesTest");
        await FileDatabase.AddFileAsync(new());
        Assert.Equal(1, await FileDatabase.GetFilesCountAsync());
        await FileDatabase.DeleteFilesAsync(dir => true);
        Assert.Equal(0, await FileDatabase.GetFilesCountAsync());
    }

    [Fact]
    public async Task FileCountTest()
    {
        Helper.Prepare("FileCountTest");

        for (var i = 0; i < 5; i++) await FileDatabase.AddFileAsync(new());
        var directories = await FileDatabase.GetFilesAsync();

        Assert.Equal(5, directories.Count);
        Assert.Equal(5, await FileDatabase.GetFilesCountAsync());
    }
}