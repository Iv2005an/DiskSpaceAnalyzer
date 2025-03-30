using static DiskSpaceAnalyzerLib.Models.Category;

namespace DiskSpaceAnalyzerLib.Models;

public class CategoryInfo
{
    public required Categories Category { get; init; }
    public required List<AnalyzedFile> Files { get; init; }
    public required List<List<AnalyzedFile>> Duplicates { get; init; }
    public int Count => Files.Count + Duplicates.Sum(list => list.Count);
    public int ClearCount => Files.Count + Duplicates.Count;
    public long Weight => Files.Sum(file => file.Weight) + Duplicates.Sum(list => list.Sum(file => file.Weight));
    public long ClearWeight => Files.Sum(file => file.Weight) + Duplicates.Sum(list => list[0].Weight);
    public float CountPercentages { get; private set; }
    public float WeightPercentages { get; private set; }

    public void CalculatePercentages(int allFilesCount, long allFileWeights)
    {
        CountPercentages = (float)Math.Round(
            (float)(Files.Count + Duplicates.Count) / allFilesCount, 4);
        WeightPercentages = (float)Math.Round(
            (float)Weight / allFileWeights, 4);
    }

    public override string ToString() => Category.ToString();
}