using DiskSpaceAnalyzerLib.Extensions;
using DiskSpaceAnalyzerLib.Services;
using SQLite;
using static DiskSpaceAnalyzerLib.Models.Category;

namespace DiskSpaceAnalyzerLib.Models;

[Table("analyzed_files")]
public class AnalyzedFile
{
    [Ignore]
    public FileInfo File
    {
        get => new(Path.Combine(DirectoryPath, Name));
        init
        {
            DirectoryPath = value.DirectoryName!;
            Name = value.Name;
            AnalyzeTimeUtc = DateTime.UtcNow;
            Weight = value.Length;
            Category = value.GetCategory();
            EditTimeUtc = value.LastWriteTimeUtc;
            Checksum = FileService.GetChecksum(value);
        }
    }

    [Column("id")]
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; init; }

    [Column("directory_path")] public string DirectoryPath { get; init; } = "";

    [Column("name")] public string Name { get; init; } = "";

    [Column("analyze_time_utc")] public DateTime AnalyzeTimeUtc { get; init; }

    [Column("weight")] public long Weight { get; init; }

    [Column("category")] public Categories Category { get; init; }

    [Column("edit_time")] public DateTime EditTimeUtc { get; init; }

    [Column("checksum")] public string Checksum { get; init; } = "";

    public override string ToString() => File.FullName;
}