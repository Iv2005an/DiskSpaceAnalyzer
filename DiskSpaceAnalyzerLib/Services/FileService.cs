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

    static bool CompareFiles(FileInfo file1, FileInfo file2)
    {
        if (file1.FullName == file2.FullName) return true;
        if (file1.Length != file2.Length) return false;
        using FileStream fileStream1 = file1.OpenRead();
        using FileStream fileStream2 = file2.OpenRead();
        int fileByte1;
        int fileByte2;
        do
        {
            fileByte1 = fileStream1.ReadByte();
            fileByte2 = fileStream2.ReadByte();
        }
        while ((fileByte1 == fileByte2) && (fileByte1 != -1));
        return (fileByte1 - fileByte2) == 0;
    }

    public static List<List<AnalyzedFile>> GetFileDuplicates(
        List<AnalyzedFile> files,
        bool isFastCompare = true, bool isFileNameCompare = false)
    {
        List<List<AnalyzedFile>> duplicates = [];
        for (int i = 0; i < files.Count; i++)
        {
            AnalyzedFile analyzedFile = files[i];
            List<AnalyzedFile> fileDuplicates = files.FindAll(
                file => (!isFileNameCompare || file.Name == analyzedFile.Name)
                && file.Weight == analyzedFile.Weight
                && file.Category == analyzedFile.Category
                && file.Checksum == analyzedFile.Checksum);
            if (!isFastCompare) fileDuplicates = fileDuplicates.FindAll(file => CompareFiles(file.File, analyzedFile.File));
            if (fileDuplicates.Count > 1)
            {
                duplicates.Add(fileDuplicates);
                files.RemoveAll(file => fileDuplicates.Contains(file));
                i--;
            }
        }
        return duplicates;
    }

    static string GetNewFileName(FileInfo file, DirectoryInfo newDir)
    {
        string newFilePath = Path.Combine(newDir.FullName, file.Name);
        if (File.Exists(newFilePath))
            for (int i = 1; ; i++)
            {
                newFilePath = Path.Combine(newDir.FullName, $"{Path.GetFileNameWithoutExtension(file.Name)}({i}){file.Extension}");
                if (!File.Exists(newFilePath)) break;
            }
        return newFilePath;
    }

    public static void Copy(
        AnalyzedFile analyzedFile,
        DirectoryInfo outputDir,
        IProgress<FileProgressReport>? fileProgressReport = null)
    {
        try
        {
            if (!outputDir.Exists)
                outputDir.Create();
            long availableFreeSpace = new DriveInfo(outputDir.Root.FullName).AvailableFreeSpace;
            if (analyzedFile.Weight > availableFreeSpace)
            {
                fileProgressReport?.Report(new(analyzedFile.File, "NOT ENOUGH SPACE", ReportLevel.ERROR));
                return;
            }
            string newFilePath = GetNewFileName(analyzedFile.File, outputDir);
            analyzedFile.File.CopyTo(newFilePath);
            fileProgressReport?.Report(new(analyzedFile.File, "COPIED", ReportLevel.SUCCESS));
        }
        catch (IOException) { fileProgressReport?.Report(new(analyzedFile.File, "I/O ERROR", ReportLevel.ERROR)); }
        catch (System.Security.SecurityException) { fileProgressReport?.Report(new(analyzedFile.File, "SECURITY ERROR", ReportLevel.ERROR)); }
        catch (UnauthorizedAccessException) { fileProgressReport?.Report(new(analyzedFile.File, "ACCESS ERROR", ReportLevel.ERROR)); }
        catch (Exception) { fileProgressReport?.Report(new(analyzedFile.File, "INVALID ERROR", ReportLevel.ERROR)); }
    }
}
