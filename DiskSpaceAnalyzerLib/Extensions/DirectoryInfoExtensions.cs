namespace DiskSpaceAnalyzerLib.Extensions;

public static class DirectoryInfoExtensions
{
    public static bool IsChildDirectoryOf(this DirectoryInfo childDir, DirectoryInfo parentDir)
    {
        while (childDir.Parent != null)
        {
            if (childDir.Parent.FullName == parentDir.FullName) return true;
            else childDir = childDir.Parent;
        }
        return false;
    }
}
