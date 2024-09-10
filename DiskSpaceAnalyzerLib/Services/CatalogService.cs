using DiskSpaceAnalyzerLib.Models;

namespace DiskSpaceAnalyzerLib.Services;

public static class CatalogService
{
    static bool CompareFiles(string filePath1, string filePath2)
    {
        if (filePath1 == filePath2) return true;
        using FileStream fileStream1 = new(filePath1, FileMode.Open, FileAccess.Read);
        using FileStream fileStream2 = new(filePath2, FileMode.Open, FileAccess.Read);
        if (fileStream1.Length != fileStream2.Length) return false;
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
    static string? GetNewFilePath(string newFilePathDirectory, string sourceFilePath)
    {
        string newFilePath = Path.Combine(newFilePathDirectory, Path.GetFileName(sourceFilePath));
        if (File.Exists(newFilePath))
            if (!CompareFiles(sourceFilePath, newFilePath))
                for (int i = 1; ; i++)
                {
                    newFilePath = $"{Path.Combine(newFilePathDirectory,
                        Path.GetFileNameWithoutExtension(newFilePath))}({i}){Path.GetExtension(newFilePath)}";
                    if (!File.Exists(newFilePath)) break;
                    if (CompareFiles(sourceFilePath, newFilePath)) return null;
                }
            else return null;
        else if (!Directory.Exists(newFilePathDirectory))
            Directory.CreateDirectory(newFilePathDirectory);
        return newFilePath;
    }
    public static void Sort(List<AnalyzedFile> filesToSort, string pathToSave,
        Action<AnalyzedFile, string>? onSuccessCopy = null,
        Action<AnalyzedFile, string>? onDuplicate = null,
        Action<AnalyzedFile, Exception>? onException = null)
    {
        foreach (AnalyzedFile file in filesToSort)
        {
            string filePath = Path.Combine(file.DirectoryPath, file.Name);
            string newFilePathDirectory = Path.Combine(pathToSave, "DataSpaceAnalyzerSortedData", file.Type.ToString());
            string? newFilePath = GetNewFilePath(newFilePathDirectory, filePath);
            if (newFilePath is not null)
            {
                try
                {
                    File.Copy(filePath, newFilePath);
                    onSuccessCopy?.Invoke(file, newFilePath);
                }
                catch (Exception ex) { onException?.Invoke(file, ex); }
            }
            else onDuplicate?.Invoke(file, newFilePathDirectory);
        }
    }
}
