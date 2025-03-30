using SQLite;

namespace DiskSpaceAnalyzerLib.Models;

[Table("analyzed_directories")]
public class AnalyzedDirectory
{
    [Ignore]
    public DirectoryInfo Directory
    {
        get => new(DirectoryPath);
        init
        {
            DirectoryPath = value.FullName;
            AnalyzeTimeUtc = DateTime.UtcNow;
        }
    }

    [Column("id")]
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; init; }

    [Column("directory_path")]
    [Indexed(Unique = true)]
    public string DirectoryPath { get; init; } = "";

    [Column("analyze_time_utc")] public DateTime AnalyzeTimeUtc { get; init; }

    [Column("file_count")] public int FileCount { get; set; }

    [Column("directory_count")] public int DirectoryCount { get; set; }

    [Column("files_weight")] public long FilesWeight { get; set; }

    public override string ToString() => Directory.FullName;
}