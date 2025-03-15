using SQLite;

namespace DiskSpaceAnalyzerLib.Models;

[Table("analyzed_directories")]
public class AnalyzedDirectory
{
    [Ignore]
    public DirectoryInfo Directory
    {
        get => new(DirectoryPath);
        set
        {
            DirectoryPath = value.FullName;
            AnalyzeTimeUtc = DateTime.UtcNow;
        }
    }

    [Column("id"), PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    [Column("directory_path"), Indexed(Unique = true)]
    public string DirectoryPath { get; set; } = "";

    [Column("analyze_time_utc")]
    public DateTime AnalyzeTimeUtc { get; set; }

    [Column("file_count")]
    public int FileCount { get; set; }

    [Column("directory_count")]
    public int DirectoryCount { get; set; }

    [Column("files_weight")]
    public long FilesWeight { get; set; }
}
