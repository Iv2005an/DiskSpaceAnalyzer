using static DiskSpaceAnalyzerLib.Constants;
using static DiskSpaceAnalyzerLib.Models.FileType;

namespace DiskSpaceAnalyzerTest;

public class FileTypeTest
{
    [Fact]
    public void GetFileTypeTest()
    {
        Assert.Equal(FileTypes.Raster, GetFileType("filePath.jpg"));
        Assert.Equal(FileTypes.Vector, GetFileType("filePath.svg"));
        Assert.Equal(FileTypes.Text, GetFileType("filePath.docx"));
        Assert.Equal(FileTypes.Audio, GetFileType("filePath.mp3"));
        Assert.Equal(FileTypes.Video, GetFileType("filePath.mp4"));
        Assert.Equal(FileTypes.EBook, GetFileType("filePath.fb2"));
        Assert.Equal(FileTypes.CAD, GetFileType("filePath.3dm"));
        Assert.Equal(FileTypes.Presentation, GetFileType("filePath.pptx"));
        Assert.Equal(FileTypes.Spreadsheet, GetFileType("filePath.xlsx"));
        Assert.Equal(FileTypes.Database, GetFileType("filePath.db3"));
        Assert.Equal(FileTypes.Archive, GetFileType("filePath.7z"));
        Assert.Equal(FileTypes.Web, GetFileType("filePath.html"));
        Assert.Equal(FileTypes.Developer, GetFileType("filePath.c"));
        Assert.Equal(FileTypes.System, GetFileType("filePath.dll"));
        Assert.Equal(FileTypes.Executables, GetFileType("filePath.exe"));
        Assert.Equal(FileTypes.Settings, GetFileType("filePath.cfg"));
        Assert.Equal(FileTypes.Other, GetFileType("filePath.as32d"));
    }
}