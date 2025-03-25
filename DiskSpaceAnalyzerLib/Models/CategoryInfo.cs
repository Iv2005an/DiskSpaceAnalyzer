using static DiskSpaceAnalyzerLib.Models.Category;

namespace DiskSpaceAnalyzerLib.Models;

public class CategoryInfo(
    Categories category, float percentages,
    List<AnalyzedFile> files, List<List<AnalyzedFile>> duplicates)
{
    public Categories Category => category;
    public float Percentages { get; set; } = percentages;
    public List<AnalyzedFile> Files => files;
    public List<List<AnalyzedFile>> Duplicates => duplicates;
    public long Weight => Files.Sum(file => file.Weight) + Duplicates.Sum(list => list.Sum(file => file.Weight));
    public long ClearWeight => Files.Sum(file => file.Weight) + Duplicates.Sum(list => list[0].Weight);

    public override string ToString() => Category.ToString();
}
