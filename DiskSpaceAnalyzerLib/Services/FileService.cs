using System.Security.Cryptography;
using DiskSpaceAnalyzerLib.Models;

namespace DiskSpaceAnalyzerLib.Services;

public static class FileService
{
    static readonly SHA256 sha256 = SHA256.Create();

    public static string GetChecksum(FileInfo file)
    {
        using FileStream stream = file.OpenRead();
        byte[] checksum = sha256.ComputeHash(stream);
        return BitConverter.ToString(checksum).Replace("-", "");
    }
    public static bool IsIgnored(DirectoryInfo dir, List<DirectoryInfo> ignoreDirs)
    {
        foreach (DirectoryInfo ignoreDir in ignoreDirs)
            if (dir.FullName.StartsWith(ignoreDir.FullName)) return true;
        return false;
    }
}
