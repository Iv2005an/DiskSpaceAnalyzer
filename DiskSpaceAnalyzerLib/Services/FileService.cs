using System.Security;
using System.Security.Cryptography;
using DiskSpaceAnalyzerLib.Models;

namespace DiskSpaceAnalyzerLib.Services;

public static class FileService
{
    private static readonly SHA256 Sha256 = SHA256.Create();

    public static string GetChecksum(FileInfo file)
    {
        using var stream = file.OpenRead();
        var checksum = Sha256.ComputeHash(stream);
        return Convert.ToHexString(checksum);
    }

    private static bool CompareFiles(FileInfo file1, FileInfo file2)
    {
        if (file1.FullName == file2.FullName) return true;
        if (file1.Length != file2.Length) return false;
        using var fileStream1 = file1.OpenRead();
        using var fileStream2 = file2.OpenRead();
        int fileByte1;
        int fileByte2;
        do
        {
            fileByte1 = fileStream1.ReadByte();
            fileByte2 = fileStream2.ReadByte();
        } while (fileByte1 == fileByte2 && fileByte1 != -1);

        return fileByte1 - fileByte2 == 0;
    }

    public static List<List<AnalyzedFile>> GetFileDuplicates(
        List<AnalyzedFile> files,
        bool isFastCompare = true, bool isFileNameCompare = false)
    {
        List<List<AnalyzedFile>> duplicates = [];
        for (var i = 0; i < files.Count; i++)
        {
            var analyzedFile = files[i];
            var fileDuplicates = files.FindAll(
                file => (!isFileNameCompare || file.Name == analyzedFile.Name)
                        && file.Weight == analyzedFile.Weight
                        && file.Category == analyzedFile.Category
                        && file.Checksum == analyzedFile.Checksum);
            if (!isFastCompare)
                fileDuplicates = fileDuplicates.FindAll(file => CompareFiles(file.File, analyzedFile.File));
            if (fileDuplicates.Count <= 1) continue;
            {
                duplicates.Add(fileDuplicates);
                files.RemoveAll(file => fileDuplicates.Contains(file));
                i--;
            }
        }

        return duplicates;
    }

    private static string GetNewFileName(FileInfo file, DirectoryInfo newDir)
    {
        var newFilePath = Path.Combine(newDir.FullName, file.Name);
        if (!File.Exists(newFilePath)) return newFilePath;
        for (var i = 1;; i++)
        {
            newFilePath = Path.Combine(newDir.FullName,
                $"{Path.GetFileNameWithoutExtension(file.Name)}({i}){file.Extension}");
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
            var availableFreeSpace = new DriveInfo(outputDir.Root.FullName).AvailableFreeSpace;
            if (analyzedFile.Weight > availableFreeSpace)
            {
                fileProgressReport?.Report(new(analyzedFile.File, "NOT ENOUGH SPACE", ReportLevel.Error));
                return;
            }

            var newFilePath = GetNewFileName(analyzedFile.File, outputDir);
            analyzedFile.File.CopyTo(newFilePath);
            fileProgressReport?.Report(new(analyzedFile.File, "COPIED", ReportLevel.Success));
        }
        catch (IOException)
        {
            fileProgressReport?.Report(new(analyzedFile.File, "I/O ERROR", ReportLevel.Error));
        }
        catch (SecurityException)
        {
            fileProgressReport?.Report(new(analyzedFile.File, "SECURITY ERROR", ReportLevel.Error));
        }
        catch (UnauthorizedAccessException)
        {
            fileProgressReport?.Report(new(analyzedFile.File, "ACCESS ERROR", ReportLevel.Error));
        }
        catch (Exception)
        {
            fileProgressReport?.Report(new(analyzedFile.File, "INVALID ERROR", ReportLevel.Error));
        }
    }
}