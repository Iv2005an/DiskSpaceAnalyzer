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
        DirectoryInfo parentDir = new(parentPath);
        DirectoryInfo childDir = new(childPath);

        Assert.Equal(expectedResult, childDir.IsChildDirectoryOf(parentDir));
    }

    [Theory]
    [InlineData(@"C:\Test", @"C:\Test\Child", true)] // Windows прямой потомок
    [InlineData(@"C:\Test", @"C:\Test\Child\Grandchild", true)] // Windows косвенный потомок
    [InlineData("/home/user/test", "/home/user/test/subdir", true)] // Linux прямой потомок
    [InlineData("/home/user/test", "/home/user/test/subdir/grandchild", true)] // Linux косвенный потомок
    [InlineData("/Users/user/test", "/Users/user/test/subdir", true)] // macOS прямой потомок
    [InlineData("/Users/user/test", "/Users/user/test/subdir/grandchild", true)] // macOS косвенный потомок
    [InlineData(@"C:\Test", @"D:\Unrelated", false)] // Windows несвязанная директория
    [InlineData("/home/user/test", "/var/log", false)] // Linux несвязанная директория
    [InlineData("/Users/user/test", "/Library/Frameworks", false)] // macOS несвязанная директория
    public void IsParentDirectoryOfAnyTest(string parentPath, string childPath, bool expected)
    {
        var parentDir = new DirectoryInfo(parentPath);
        DirectoryInfo? dir = new(childPath);

        Assert.Equal(expected, parentDir.IsParentDirectoryOf(dir));
    }
}