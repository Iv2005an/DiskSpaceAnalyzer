using SQLite;
using DiskSpaceAnalyzerLib.Extensions;
using DiskSpaceAnalyzerLib.Services;
using static DiskSpaceAnalyzerLib.Models.Category;

namespace DiskSpaceAnalyzerLib.Models;

[Table("analyzed_files")]
public class AnalyzedFile
{
    [Ignore]
    public FileInfo File
    {
        get => new(Path.Combine(DirectoryPath, Name));
        set
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

    [Column("id"), PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    [Column("directory_path")]
    public string DirectoryPath { get; set; } = "";

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("analyze_time_utc")]
    public DateTime AnalyzeTimeUtc { get; set; }

    [Column("weight")]
    public long Weight { get; set; }

    [Column("category")]
    public Categories Category { get; set; }

    [Column("edit_time")]
    public DateTime EditTimeUtc { get; set; }

    [Column("checksum")]
    public string Checksum { get; set; } = "";

    public override string ToString() => File.FullName;
}
