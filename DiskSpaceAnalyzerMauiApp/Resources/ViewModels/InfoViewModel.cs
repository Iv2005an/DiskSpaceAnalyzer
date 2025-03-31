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

    [ObservableProperty] private ObservableCollection<OrganizeCategory> _organizeCategories = [];

    [ObservableProperty] private string _outputPath = "Выбирете папку сохранения";
    [ObservableProperty] private string _requiredDiskSpace = "";

    [ObservableProperty] private bool _canOrganize;

    [RelayCommand]
    private async Task GetInfo()
    {
        CanOrganize = false;

        OrganizeCategories = [];
        OutputPath = "Выбирете папку сохранения";
        RequiredDiskSpace = "";

        var dirs = InputPaths.Select(path => new DirectoryInfo(path)).ToList();
        var ignoreDirs = InputIgnorePaths.Select(path => new DirectoryInfo(path)).ToList();
        var categoryInfos = await DirectoryService.GetInfo(dirs, ignoreDirs);
        categoryInfos = categoryInfos.Where(c => c.Count > 0).ToList();
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
        SelectedCategories = selectedCategories.Select(oc => oc.CategoryInfo).ToList();
        var requiredSpaceData = selectedCategories.Sum(oc => oc.CategoryInfo.ClearWeight);
        RequiredDiskSpace = CategoryData.GetReadableWeight(requiredSpaceData);
        CanOrganize = OutputPath != "Выбирете папку сохранения" && selectedCategories.Count > 0;
    }
}