namespace DiskSpaceAnalyzerLib.Extensions;

public static class DirectoryInfoExtensions
{
    public static bool IsChildDirectoryOf(this DirectoryInfo childDir, DirectoryInfo parentDir)
    {
        if (childDir.FullName == parentDir.FullName) return true;
        while (childDir.Parent != null)
        {
            if (childDir.Parent.FullName == parentDir.FullName) return true;
            childDir = childDir.Parent;
        }

        return false;
    }

    public static bool IsChildDirectoryOfAny(this DirectoryInfo childDir, List<DirectoryInfo>? parentDirs) =>
        parentDirs is not null && parentDirs.Any(childDir.IsChildDirectoryOf);

    public static bool IsParentDirectoryOfAny(this DirectoryInfo parentDir, List<DirectoryInfo>? childDirs) =>
        childDirs is not null && childDirs.Any(childDir => childDir.IsChildDirectoryOf(parentDir));
}