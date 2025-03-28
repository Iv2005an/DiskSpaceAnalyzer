using static DiskSpaceAnalyzerLib.Models.Category;

namespace DiskSpaceAnalyzerLib.Models;

public class CategoryInfo(
    Categories category,
    float countPercentages, float weightPercentages,
    long weight, long clearWeight,
    List<AnalyzedFile> files, List<List<AnalyzedFile>> duplicates)
{
    public Categories Category => category;
    public float CountPercentages { get; set; } = countPercentages;
    public float WeightPercentages { get; set; } = weightPercentages;
    public List<AnalyzedFile> Files => files;
    public List<List<AnalyzedFile>> Duplicates => duplicates;
    public long Weight => weight;
    public long ClearWeight => clearWeight;

    public override string ToString() => Category.ToString();
}
