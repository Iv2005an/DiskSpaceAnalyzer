using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiskSpaceAnalyzerLib.Models;
using DiskSpaceAnalyzerLib.Services;

namespace DiskSpaceAnalyzerMauiApp.Resources.ViewModels;

public partial class AnalyzeViewModel : ObservableObject, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _inputPathsToAnalyze = query["pathsToAnalyze"] as List<string> ?? [];
        InputPaths = query["paths"] as List<string> ?? [];
        InputIgnorePaths = query["ignorePaths"] as List<string> ?? [];
        AnalyzeCommand.ExecuteAsync(null);
    }

    private List<string> _inputPathsToAnalyze = [];
    public List<string> InputPaths = [];
    public List<string> InputIgnorePaths = [];

    [ObservableProperty] public partial ObservableCollection<string> AnalyzedPaths { get; set; } = [];

    [ObservableProperty] public partial ObservableCollection<string> IgnoredPaths { get; set; } = [];

    [ObservableProperty] public partial ObservableCollection<string> ErrorPaths { get; set; } = [];

    [ObservableProperty] public partial double AnalyzeProgressValue { get; set; }

    [ObservableProperty] public partial int AnalyzedDirsCount { get; set; }

    [ObservableProperty] public partial int AllDirsCount { get; set; }

    [ObservableProperty] public partial bool CanOrganize { get; set; }

    [RelayCommand]
    private async Task Analyze()
    {
        CanOrganize = false;

        AnalyzedPaths = [];
        IgnoredPaths = [];
        ErrorPaths = [];

        AnalyzeProgressValue = 0;
        AnalyzedDirsCount = 0;
        AllDirsCount = 1;

        var dirsToAnalyze = _inputPathsToAnalyze.Select(path => new DirectoryInfo(path)).ToList();
        var ignoreDirsToAnalyze = InputIgnorePaths.Select(path => new DirectoryInfo(path)).ToList();
        var progress = new Progress<DirectoryProgressReport>();
        progress.ProgressChanged += (s, e) =>
        {
            switch (e)
            {
                case { Level: ReportLevel.Success, Message: "ANALYZED" }:
                    AnalyzedPaths.Add(e.Dir.DirectoryPath);
                    break;
                case { Level: ReportLevel.Info, Message: "IGNORED" }:
                    IgnoredPaths.Add(e.Dir.DirectoryPath);
                    break;
                default:
                    {
                        if (e.Level == ReportLevel.Error)
                            ErrorPaths.Add($"{e.Message}: {e.Dir.DirectoryPath}");
                        break;
                    }
            }

            AllDirsCount += e.Dir.DirectoryCount;
            AnalyzedDirsCount += 1;
            AnalyzeProgressValue = (double)AnalyzedDirsCount / AllDirsCount;
        };
        await DirectoryService.Analyze(dirsToAnalyze, ignoreDirsToAnalyze, progress);
        CanOrganize = true;
    }
}