using System.Security.Cryptography;
using static DiskSpaceAnalyzerLib.Models.Category;
using static DiskSpaceAnalyzerLib.Models.Category.Categories;

namespace DiskSpaceAnalyzerLib.Extensions;

public static class FileInfoExtensions
{
    private static readonly MD5 Md5 = MD5.Create();

    public static Categories GetCategory(this FileInfo file)
    {
        var extension = file.Extension.Replace(".", "").ToUpper();
        if (Raster.GetExtensions().Contains(extension)) return Raster;
        if (Vector.GetExtensions().Contains(extension)) return Vector;
        if (Text.GetExtensions().Contains(extension)) return Text;
        if (Audio.GetExtensions().Contains(extension)) return Audio;
        if (Video.GetExtensions().Contains(extension)) return Video;
        if (EBook.GetExtensions().Contains(extension)) return EBook;
        if (Cad.GetExtensions().Contains(extension)) return Cad;
        if (Presentation.GetExtensions().Contains(extension)) return Presentation;
        if (Spreadsheet.GetExtensions().Contains(extension)) return Spreadsheet;
        if (Database.GetExtensions().Contains(extension)) return Database;
        if (Archive.GetExtensions().Contains(extension)) return Archive;
        if (Web.GetExtensions().Contains(extension)) return Web;
        if (Developer.GetExtensions().Contains(extension)) return Developer;
        if (Categories.System.GetExtensions().Contains(extension)) return Categories.System;
        if (Executables.GetExtensions().Contains(extension)) return Executables;
        if (Settings.GetExtensions().Contains(extension)) return Settings;
        return Other;
    }

    public static string GetChecksum(this FileInfo file)
    {
        using var stream = file.OpenRead();
        var checksum = Md5.ComputeHash(stream);
        return Convert.ToHexString(checksum);
    }

    public static bool CompareFiles(this FileInfo file1, FileInfo file2)
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

    public static string GetNewFileName(this FileInfo file, DirectoryInfo newDir)
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
}