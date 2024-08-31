using SQLite;

namespace DiskSpaceAnalyzerLib.Models;

[Table("analyzed_directories")]
public class AnalyzedDirectory
{
    public AnalyzedDirectory() { }
    public AnalyzedDirectory(string path) => DirectoryPath = System.IO.Path.GetFullPath(path);

    [Column("id"), PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    [Column("directory_path"), Indexed(Unique = true)]
    public string DirectoryPath { get; set; } = "";
}
