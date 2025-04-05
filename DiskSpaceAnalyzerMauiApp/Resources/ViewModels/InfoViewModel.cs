using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiskSpaceAnalyzerLib.Models;
using DiskSpaceAnalyzerLib.Services;
using DiskSpaceAnalyzerMauiApp.Resources.Models;

namespace DiskSpaceAnalyzerMauiApp.Resources.ViewModels;

public partial class InfoViewModel : ObservableObject, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        InputPaths = query["paths"] as List<string> ?? [];
        InputIgnorePaths = query["ignorePaths"] as List<string> ?? [];
        GetInfoCommand.ExecuteAsync(null);
    }

    public List<string> InputPaths = [];
    public List<string> InputIgnorePaths = [];
    public List<CategoryInfo> SelectedCategories = [];

    [ObservableProperty] public partial ObservableCollection<OrganizeCategory> OrganizeCategories { get; set; } = [];
    [ObservableProperty] public partial string? OutputPath { get; set; }
    [ObservableProperty] public partial string RequiredDiskSpace { get; set; }

    [ObservableProperty] public partial bool CanOrganize { get; set; }


    [RelayCommand]
    private async Task GetInfo()
    {
        CanOrganize = false;

        OrganizeCategories = [];
        OutputPath = null;
        RequiredDiskSpace = "";

        var dirs = InputPaths.Select(path => new DirectoryInfo(path)).ToList();
        var ignoreDirs = InputIgnorePaths.Select(path => new DirectoryInfo(path)).ToList();
        var categoryInfos = await DirectoryService.GetInfo(dirs, ignoreDirs);
        categoryInfos = [.. categoryInfos.Where(c => c.Count > 0)];
        categoryInfos.Sort((a, b) => a.WeightPercentages < b.WeightPercentages ? 1 : -1);
        foreach (var organizeCategory in categoryInfos.Select(categoryInfo => new OrganizeCategory(categoryInfo)))
        {
            organizeCategory.SelectedChanged += _ => ComputeRequiredSpace();
            OrganizeCategories.Add(organizeCategory);
        }

        ComputeRequiredSpace();
    }

    [RelayCommand]
    private async Task SelectFolder(CancellationToken cancellationToken)
    {
        var path = await PickFolder(cancellationToken);
        if (path is null) return;
        OutputPath = path;
        ComputeRequiredSpace();
    }

    [RelayCommand]
    private static async Task<string?> PickFolder(CancellationToken cancellationToken)
    {
        var folderPickerResult = await FolderPicker.PickAsync(cancellationToken);
        return folderPickerResult.IsSuccessful ? folderPickerResult.Folder.Path : null;
    }

    private void ComputeRequiredSpace()
    {
        var selectedCategories = OrganizeCategories.Where(oc => oc.Selected).ToList();
        SelectedCategories = [.. selectedCategories.Select(oc => oc.CategoryInfo)];
        var requiredSpaceData = selectedCategories.Sum(oc => oc.CategoryInfo.ClearWeight);
        RequiredDiskSpace = CategoryData.GetReadableWeight(requiredSpaceData);
        CanOrganize = OutputPath is not null && selectedCategories.Count > 0;
    }
}