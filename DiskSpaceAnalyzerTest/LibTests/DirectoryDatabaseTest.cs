using DiskSpaceAnalyzerLib.Databases;

namespace DiskSpaceAnalyzerTest.LibTests;

public class DirectoryDatabaseTest
{
    [Fact]
    public async Task AddDirectoryTest()
    {
        Helper.Prepare("AddDirectoryTest");
        await DirectoryDatabase.AddDirectoryAsync(new());
        Assert.Equal(1, await DirectoryDatabase.GetDirectoriesCountAsync());
    }

    [Fact]
    public async Task DeleteDirectoriesTest()
    {
        Helper.Prepare("DeleteDirectoriesTest");
        await DirectoryDatabase.AddDirectoryAsync(new());
        Assert.Equal(1, await DirectoryDatabase.GetDirectoriesCountAsync());
        await DirectoryDatabase.DeleteDirectoriesAsync(dir => true);
        Assert.Equal(0, await DirectoryDatabase.GetDirectoriesCountAsync());
    }

    [Fact]
    public async Task DirectoryCountTest()
    {
        Helper.Prepare("DirectoryCountTest");

        for (var i = 0; i < 5; i++) await DirectoryDatabase.AddDirectoryAsync(new() { Directory = new(i.ToString()) });
        var directories = await DirectoryDatabase.GetDirectoriesAsync();

        Assert.Equal(5, directories.Count);
        Assert.Equal(5, await DirectoryDatabase.GetDirectoriesCountAsync());
    }
}