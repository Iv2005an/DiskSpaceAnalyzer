using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DiskSpaceAnalyzerLib.Databases;
using DiskSpaceAnalyzerLib.Extensions;
using DiskSpaceAnalyzerLib.Models;

namespace DiskSpaceAnalyzerMauiApp.Resources.ViewModels;

public partial class AnalyzeSettingsViewModel : ObservableObject
{
    public AnalyzeSettingsViewModel() => SourcePaths.CollectionChanged += (_, _) => IsCanAnalyze();

    public IProgress<ProgressReport>? Progress { private get; set; }

    [ObservableProperty] public partial bool CanAnalyze { get; set; }

    [ObservableProperty] public partial bool RepeatAnalyze { get; set; }

    [ObservableProperty] public partial ObservableCollection<string> SourcePaths { get; set; } = [];
    [ObservableProperty] public partial ObservableCollection<string> IgnorePaths { get; set; } = [];

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(DeletePathCommand))]
    public partial string? SelectedSourcePath { get; set; }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(DeleteIgnorePathCommand))]
    public partial string? SelectedIgnorePath { get; set; }

    [RelayCommand]
    private async Task AddPath(CancellationToken cancellationToken)
    {
        var path = await PickFolder(cancellationToken);
        if (path is null) return;
        if (SourcePaths.Contains(path))
        {
            Progress?.Report(new("Директория уже добавлена", ReportLevel.Error));
            return;
        }

        var dir = new DirectoryInfo(path);
        var dirs = SourcePaths.Select(d => new DirectoryInfo(d)).ToList();
        if (dir.IsChildDirectoryOf(dirs))
        {
            Progress?.Report(new("Директория является дочерней для одной из добавленных", ReportLevel.Error));
            return;
        }

        if (dir.IsParentDirectoryOf(dirs))
        {
            SourcePaths =
            [
                .. dirs.Where(d => !d.IsChildDirectoryOf(dir))
                    .Select(d => d.FullName).ToList()
            ];
            Progress?.Report(new("Директории объединены"));
        }

        var ignoreDirs = IgnorePaths.Select(d => new DirectoryInfo(d)).ToList();
        if (dir.IsChildDirectoryOf(ignoreDirs))
        {
            IgnorePaths =
            [
                .. ignoreDirs.Where(d => !dir.IsChildDirectoryOf(d))
                    .Select(d => d.FullName).ToList()
            ];
            Progress?.Report(new("Конфликтующие директории для игнорирования удалены"));
        }

        SourcePaths.Add(path);
    }

    [RelayCommand]
    private async Task AddIgnorePath(CancellationToken cancellationToken)
    {
        var path = await PickFolder(cancellationToken);
        if (path is null) return;
        if (IgnorePaths.Contains(path))
        {
            Progress?.Report(new("Директория уже добавлена", ReportLevel.Error));
            return;
        }

        var ignoreDir = new DirectoryInfo(path);
        var ignoreDirs = IgnorePaths.Select(d => new DirectoryInfo(d)).ToList();
        if (ignoreDir.IsChildDirectoryOf(ignoreDirs))
        {
            Progress?.Report(new("Директория является дочерней для одной из добавленных", ReportLevel.Error));
            return;
        }

        if (ignoreDir.IsParentDirectoryOf(ignoreDirs))
        {
            IgnorePaths =
            [
                .. ignoreDirs.Where(d => !d.IsChildDirectoryOf(ignoreDir))
                    .Select(d => d.FullName).ToList()
            ];
            Progress?.Report(new("Директории объединены"));
        }

        var dirs = SourcePaths.Select(d => new DirectoryInfo(d)).ToList();
        if (ignoreDir.IsParentDirectoryOf(dirs))
        {
            SourcePaths =
            [
                .. dirs.Where(d => !d.IsChildDirectoryOf(ignoreDir))
                    .Select(d => d.FullName).ToList()
            ];
            Progress?.Report(new("Конфликтующие директории удалены"));
        }

        IgnorePaths.Add(path);
    }

    [RelayCommand]
    private static async Task<string?> PickFolder(CancellationToken cancellationToken)
    {
        var folderPickerResult = await FolderPicker.PickAsync(cancellationToken);
        return folderPickerResult.IsSuccessful ? folderPickerResult.Folder.Path : null;
    }

    [RelayCommand(CanExecute = nameof(CanDeletePath))]
    private void DeletePath(string path)
    {
        SourcePaths.Remove(path);
        SelectedSourcePath = null;
    }

    private bool CanDeletePath() => SelectedSourcePath is not null;

    [RelayCommand(CanExecute = nameof(CanDeleteIgnorePath))]
    private void DeleteIgnorePath(string path)
    {
        IgnorePaths.Remove(path);
        SelectedIgnorePath = null;
    }

    private bool CanDeleteIgnorePath() => SelectedIgnorePath is not null;

    partial void OnSourcePathsChanged(ObservableCollection<string> value)
    {
        IsCanAnalyze();
        value.CollectionChanged += (_, _) => IsCanAnalyze();
    }

    private void IsCanAnalyze() => CanAnalyze = SourcePaths.Count > 0;
}