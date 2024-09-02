using DiskSpaceAnalyzerLib.Extensions;
using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerLib.Models;

public static class FileType
{
    public static FileTypes GetFileType(string filePath)
    {
        string extension = Path.GetExtension(filePath).Replace(".", null).ToUpper();
        if (FileTypes.Raster.GetExtensions().Contains(extension)) return FileTypes.Raster;
        else if (FileTypes.Vector.GetExtensions().Contains(extension)) return FileTypes.Vector;
        else if (FileTypes.Text.GetExtensions().Contains(extension)) return FileTypes.Text;
        else if (FileTypes.Audio.GetExtensions().Contains(extension)) return FileTypes.Audio;
        else if (FileTypes.Video.GetExtensions().Contains(extension)) return FileTypes.Video;
        else if (FileTypes.EBook.GetExtensions().Contains(extension)) return FileTypes.EBook;
        else if (FileTypes.CAD.GetExtensions().Contains(extension)) return FileTypes.CAD;
        else if (FileTypes.Presentation.GetExtensions().Contains(extension)) return FileTypes.Presentation;
        else if (FileTypes.Spreadsheet.GetExtensions().Contains(extension)) return FileTypes.Spreadsheet;
        else if (FileTypes.Database.GetExtensions().Contains(extension)) return FileTypes.Database;
        else if (FileTypes.Archive.GetExtensions().Contains(extension)) return FileTypes.Archive;
        else if (FileTypes.Web.GetExtensions().Contains(extension)) return FileTypes.Web;
        else if (FileTypes.Developer.GetExtensions().Contains(extension)) return FileTypes.Developer;
        else if (FileTypes.System.GetExtensions().Contains(extension)) return FileTypes.System;
        else if (FileTypes.Executables.GetExtensions().Contains(extension)) return FileTypes.Executables;
        else if (FileTypes.Settings.GetExtensions().Contains(extension)) return FileTypes.Settings;
        else return FileTypes.Other;
    }
}
