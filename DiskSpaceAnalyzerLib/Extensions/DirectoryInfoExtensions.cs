namespace DiskSpaceAnalyzerLib.Extensions;

public static class DirectoryInfoExtensions
{
    public static bool IsChildDirectoryOf(this DirectoryInfo childDir, DirectoryInfo parentDir)
    {
        if (childDir.FullName == parentDir.FullName) return true;
        while (childDir.Parent != null)
        {
            if (childDir.Parent.FullName == parentDir.FullName) return true;
            else childDir = childDir.Parent;
        }
        return false;
    }

    public static bool IsChildDirectoryOfAny(this DirectoryInfo childDir, List<DirectoryInfo>? parentDirs)
    {
        if (parentDirs is null) return false;
        foreach (DirectoryInfo parentDir in parentDirs)
            if (childDir.IsChildDirectoryOf(parentDir)) return true;
        return false;
    }

    public static bool IsParentDirectoryOfAny(this DirectoryInfo parentDir, List<DirectoryInfo>? childDirs)
    {
        if (childDirs is null) return false;
        foreach (DirectoryInfo childDir in childDirs)
            if (childDir.IsChildDirectoryOf(parentDir)) return true;
        return false;
    }
}
