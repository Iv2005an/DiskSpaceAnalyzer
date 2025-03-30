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

    [Column("file_count")] public int FileCount { get; init; }

    [Column("directory_count")] public int DirectoryCount { get; init; }

    [Column("files_weight")] public long FilesWeight { get; init; }

    public override string ToString() => Directory.FullName;
}