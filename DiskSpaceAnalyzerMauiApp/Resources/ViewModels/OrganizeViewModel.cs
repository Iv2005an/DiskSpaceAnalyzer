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

    [ObservableProperty] public partial ObservableCollection<string> OrganizedPaths { get; set; } = [];
    [ObservableProperty] public partial ObservableCollection<string> ErrorPaths { get; set; } = [];

    [ObservableProperty] public partial int OrganizedFileCount { get; set; }
    [ObservableProperty] public partial int AllFileCount { get; set; }
    [ObservableProperty] public partial double OrganizeProgressValue { get; set; }

    [ObservableProperty] public partial bool CanComplete { get; set; }

    [RelayCommand]
    private async Task Organize()
    {
        CanComplete = false;

        OrganizedPaths = [];

        OrganizeProgressValue = 0;
        OrganizedFileCount = 0;
        AllFileCount = _categoryInfos.Sum(c => c.ClearCount);

        var progress = new Progress<FileProgressReport>();
        progress.ProgressChanged += (s, e) =>
        {
            switch (e)
            {
                case { Level: ReportLevel.Success, Message: "COPIED" }:
                    OrganizedPaths.Add(e.File.File.FullName);
                    break;
                default:
                {
                    if (e.Level == ReportLevel.Error)
                    {
                        ErrorPaths.Add($"{e.Message}: {e.File.File.FullName}");
                    }

                    break;
                }
            }

            OrganizedFileCount += 1;
            OrganizeProgressValue = (double)OrganizedFileCount / AllFileCount;
        };
        await DirectoryService.Organize(new(_outputDir), _categoryInfos, progress);

        CanComplete = true;
    }
}