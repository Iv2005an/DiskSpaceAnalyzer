using static DiskSpaceAnalyzerLib.Models.Category;
using static DiskSpaceAnalyzerLib.Models.Category.Categories;

namespace DiskSpaceAnalyzerLib.Extensions;

public static class FileInfoExtensions
{
    public static Categories GetCategory(this FileInfo file)
    {
        string extension = file.Extension.Replace(".", "").ToUpper();
        if (Raster.GetExtensions().Contains(extension)) return Raster;
        if (Vector.GetExtensions().Contains(extension)) return Vector;
        if (Text.GetExtensions().Contains(extension)) return Text;
        if (Audio.GetExtensions().Contains(extension)) return Audio;
        if (Video.GetExtensions().Contains(extension)) return Video;
        if (EBook.GetExtensions().Contains(extension)) return EBook;
        if (CAD.GetExtensions().Contains(extension)) return CAD;
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
}