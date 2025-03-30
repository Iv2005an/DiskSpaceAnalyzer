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

    [ObservableProperty] private ObservableCollection<string> _analyzedPaths = [];
    [ObservableProperty] private ObservableCollection<string> _ignoredPaths = [];
    [ObservableProperty] private ObservableCollection<string> _errorPaths = [];

    [ObservableProperty] private double _analyzeProgressValue;
    [ObservableProperty] private int _analyzedDirsCount;
    [ObservableProperty] private int _allDirsCount;

    [ObservableProperty] private bool _canOrganize;


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