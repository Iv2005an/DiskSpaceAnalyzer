using CommunityToolkit.Mvvm.ComponentModel;
using DiskSpaceAnalyzerLib.Models;

namespace DiskSpaceAnalyzerMauiApp.Resources.Models;

public partial class OrganizeCategory(CategoryInfo categoryInfo) : ObservableObject
{
    public delegate void SelectedHandler(bool selected);

    public event SelectedHandler? SelectedChanged;
    public CategoryInfo CategoryInfo { get; set; } = categoryInfo;
    [ObservableProperty] public partial bool Selected { get; set; }
    public Color Color { get; private init; } = CategoryData.GetColor(categoryInfo.Category);
    public string Name { get; private init; } = CategoryData.GetName(categoryInfo.Category);
    public int FileCount { get; private init; } = categoryInfo.Count;
    public int ClearFileCount { get; private init; } = categoryInfo.ClearCount;
    public string Weight { get; private init; } = CategoryData.GetReadableWeight(categoryInfo.Weight);
    public string ClearWeight { get; private init; } = CategoryData.GetReadableWeight(categoryInfo.ClearWeight);

    partial void OnSelectedChanged(bool value)
    {
        SelectedChanged?.Invoke(value);
    }
}