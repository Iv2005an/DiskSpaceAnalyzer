using System.Security;
using DiskSpaceAnalyzerLib.Extensions;
using DiskSpaceAnalyzerLib.Models;

namespace DiskSpaceAnalyzerLib.Services;

public static class FileService
{
    public static List<List<AnalyzedFile>> GetFileDuplicates(List<AnalyzedFile> files)
    {
        List<List<AnalyzedFile>> duplicates = [];
        for (var i = 0; i < files.Count; i++)
        {
            var analyzedFile = files[i];
            var fileDuplicates = files.FindAll(
                file => file.Category == analyzedFile.Category
                        && file.Weight == analyzedFile.Weight
                        && file.EditTimeUtc == analyzedFile.EditTimeUtc
                        && file.File.CompareFiles(analyzedFile.File));
            if (fileDuplicates.Count <= 1) continue;
            duplicates.Add(fileDuplicates);
            files.RemoveAll(fileDuplicates.Contains);
            i--;
        }

        return duplicates;
    }

    public static void Copy(
        AnalyzedFile analyzedFile,
        DirectoryInfo outputDir,
        IProgress<FileProgressReport>? fileProgressReport = null)
    {
        try
        {
            if (!outputDir.Exists) outputDir.Create();
            if (DirectoryService.CheckAvailableDiskSpace(outputDir, analyzedFile.Weight))
            {
                fileProgressReport?.Report(new(analyzedFile, "NOT ENOUGH SPACE", ReportLevel.Error));
                return;
            }

            var newFileName = analyzedFile.File.GetNewFileName(outputDir);
            analyzedFile.File.CopyTo(newFileName);
            fileProgressReport?.Report(new(analyzedFile, "COPIED", ReportLevel.Success));
        }
        catch (DirectoryNotFoundException)
        {
            fileProgressReport?.Report(new(analyzedFile, "DIRECTORY NOT FOUND", ReportLevel.Error));
        }
        catch (FileNotFoundException)
        {
            fileProgressReport?.Report(new(analyzedFile, "FILE NOT FOUND", ReportLevel.Error));
        }
        catch (PathTooLongException)
        {
            fileProgressReport?.Report(new(analyzedFile, "PATH TOO LONG ERROR", ReportLevel.Error));
        }
        catch (IOException)
        {
            fileProgressReport?.Report(new(analyzedFile, "I/O ERROR", ReportLevel.Error));
        }
        catch (SecurityException)
        {
            fileProgressReport?.Report(new(analyzedFile, "SECURITY ERROR", ReportLevel.Error));
        }
        catch (UnauthorizedAccessException)
        {
            fileProgressReport?.Report(new(analyzedFile, "ACCESS ERROR", ReportLevel.Error));
        }
        catch (Exception)
        {
            fileProgressReport?.Report(new(analyzedFile, "INVALID ERROR", ReportLevel.Error));
        }
    }
}