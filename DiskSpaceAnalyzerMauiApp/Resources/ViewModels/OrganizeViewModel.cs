using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiskSpaceAnalyzerLib.Models;
using DiskSpaceAnalyzerLib.Services;

namespace DiskSpaceAnalyzerMauiApp.Resources.ViewModels;

public partial class OrganizeViewModel : ObservableObject, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        InputPaths = query["paths"] as List<string> ?? [];
        InputIgnorePaths = query["ignorePaths"] as List<string> ?? [];
        _outputDir = query["outputDir"] as string ?? "";
        _categoryInfos = query["categoryInfos"] as List<CategoryInfo> ?? [];
        OrganizeCommand.ExecuteAsync(null);
    }

    public List<string> InputPaths = [];
    public List<string> InputIgnorePaths = [];
    private string _outputDir = "";
    private List<CategoryInfo> _categoryInfos = [];

    [ObservableProperty] private ObservableCollection<string> _organizedPaths = [];

    [ObservableProperty] private double _organizeProgressValue;
    [ObservableProperty] private int _organizedFileCount;
    [ObservableProperty] private int _allFileCount;

    [ObservableProperty] private bool _canComplete;


    [RelayCommand]
    private async Task Organize()
    {
        CanComplete = false;

        OrganizedPaths = [];

        OrganizeProgressValue = 0;
        OrganizedFileCount = 0;
        AllFileCount = _categoryInfos.Sum(c => c.ClearCount);

        var dirsToOrganize = InputPaths.Select(path => new DirectoryInfo(path)).ToList();
        var ignoreDirsToOrganize = InputIgnorePaths.Select(path => new DirectoryInfo(path)).ToList();
        var progress = new Progress<FileProgressReport>();
        progress.ProgressChanged += (s, e) =>
        {
            OrganizedPaths.Add($"{e.File.FullName} · {e.Message}");
            OrganizedFileCount += 1;
            OrganizeProgressValue = (double)OrganizedFileCount / AllFileCount;
        };
        await DirectoryService.Organize(new(_outputDir), dirsToOrganize, ignoreDirsToOrganize,
            categoryInfos: _categoryInfos, fileProgressReport: progress);
        CanComplete = true;
    }
}