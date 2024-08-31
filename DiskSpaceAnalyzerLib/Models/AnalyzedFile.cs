using SQLite;
using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerLib.Models;

[Table("analyzed_files")]
public class AnalyzedFile
{
    public AnalyzedFile() { }
    public AnalyzedFile(string path, bool isError = false)
    {
        path = Path.GetFullPath(path);
        if (!isError)
        {
            DirectoryPath = Directory.GetParent(path)!.FullName;
            Name = Path.GetFileName(path);
            Type = FileType.GetFileType(path);
        }
        else
        {
            DirectoryPath = path;
            Type = FileTypes.Error;
        }
    }

    [Column("id"), PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    [Column("directory_path"), Indexed]
    public string DirectoryPath { get; set; } = "";

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("type")]
    public FileTypes Type { get; set; }
}
