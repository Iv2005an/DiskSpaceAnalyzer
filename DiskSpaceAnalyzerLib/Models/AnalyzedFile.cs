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
        set
        {
            DirectoryPath = value.DirectoryName!;
            Name = value.Name;
            Weight = value.Length;
            Category = GetCategory(value);
            EditTimeUtc = value.LastWriteTimeUtc;
            Checksum = CatalogService.GetChecksum(value);
        }
    }

    [Column("id"), PrimaryKey, AutoIncrement]
    public int ID { get; set; }

    [Column("directory_path")]
    public string DirectoryPath { get; set; } = "";

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("weight")]
    public long Weight { get; set; }

    [Column("category")]
    public Categories Category { get; set; }

    [Column("edit_time")]
    public DateTime EditTimeUtc { get; set; }

    [Column("checksum")]
    public string Checksum { get; set; } = "";
}
