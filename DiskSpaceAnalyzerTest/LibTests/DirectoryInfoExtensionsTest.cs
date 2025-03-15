using DiskSpaceAnalyzerLib.Extensions;
namespace DiskSpaceAnalyzerTest.LibTests;


public class DirectoryInfoExtensionsTests
{
    [Theory]
    [InlineData(@"C:\Parent", @"C:\Parent\Child", true)] // Windows
    [InlineData(@"C:\Parent", @"C:\Parent\Child\SubChild", true)] // Windows
    [InlineData(@"C:\Parent", @"C:\Other\Child", false)] // Windows
    [InlineData("/home/user/Parent", "/home/user/Parent/Child", true)] // Linux
    [InlineData("/home/user/Parent", "/home/user/Other/Child", false)] // Linux
    [InlineData("/Users/user/Parent", "/Users/user/Parent/Child", true)] // macOS
    [InlineData("/Users/user/Parent", "/Users/user/Other/Child", false)] // macOS
    public void IsChildDirectoryOfTest(string parentPath, string childPath, bool expectedResult)
    {
        var parentDir = new DirectoryInfo(parentPath);
        var childDir = new DirectoryInfo(childPath);

        Assert.Equal(expectedResult, childDir.IsChildDirectoryOf(parentDir));
    }
}