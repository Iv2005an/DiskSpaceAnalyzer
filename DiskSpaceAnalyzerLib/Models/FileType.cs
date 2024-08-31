using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerLib.Models
{
    public static class FileType
    {
        public static FileTypes GetFileType(string filePath)
        {
            string extension = Path.GetExtension(filePath).Replace(".", null).ToUpper();
            if (RasterImageExtensions.Contains(extension)) return FileTypes.Raster;
            else if (VectorImageExtensions.Contains(extension)) return FileTypes.Vector;
            else if (TextExtensions.Contains(extension)) return FileTypes.Text;
            else if (AudioExtensions.Contains(extension)) return FileTypes.Audio;
            else if (VideoExtensions.Contains(extension)) return FileTypes.Video;
            else if (EBookExtensions.Contains(extension)) return FileTypes.EBook;
            else if (CadExtensions.Contains(extension)) return FileTypes.CAD;
            else if (PresentationExtensions.Contains(extension)) return FileTypes.Presentation;
            else if (SpreadsheetExtensions.Contains(extension)) return FileTypes.Spreadsheet;
            else if (DatabaseExtensions.Contains(extension)) return FileTypes.Database;
            else if (ArchiveExtensions.Contains(extension)) return FileTypes.Archive;
            else if (WebExtensions.Contains(extension)) return FileTypes.Web;
            else if (DeveloperExtensions.Contains(extension)) return FileTypes.Developer;
            else if (SystemExtensions.Contains(extension)) return FileTypes.System;
            else if (ExecutablesExtensions.Contains(extension)) return FileTypes.Executables;
            else if (SettingsExtensions.Contains(extension)) return FileTypes.Settings;
            else return FileTypes.Other;
        }
    }
}
