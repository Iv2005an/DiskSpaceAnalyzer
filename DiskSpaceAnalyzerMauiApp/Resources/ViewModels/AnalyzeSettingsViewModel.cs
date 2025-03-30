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
    public AnalyzeSettingsViewModel() => Paths.CollectionChanged += (_, _) => IsCanAnalyze();

    [ObservableProperty] private bool _repeatAnalyze;
    [ObservableProperty] private bool _canAnalyze;

    public List<string> PathsToAnalyze { get; private set; } = [];
    public IProgress<ProgressReport>? Progress { private get; set; }

    [ObservableProperty] private ObservableCollection<string> _paths = [];
    [ObservableProperty] private ObservableCollection<string> _ignorePaths = [];

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(DeletePathCommand))]
    private string? _selectedPath;

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(DeleteIgnorePathCommand))]
    private string? _selectedIgnorePath;

    [RelayCommand]
    private async Task AddPath(CancellationToken cancellationToken)
    {
        var path = await PickFolder(cancellationToken);
        if (path is null) return;
        if (Paths.Contains(path))
        {
            Progress?.Report(new("Директория уже добавлена", ReportLevel.Error));
            return;
        }

        var dir = new DirectoryInfo(path);
        var dirs = Paths.Select(d => new DirectoryInfo(d)).ToList();
        if (dir.IsChildDirectoryOfAny(dirs))
        {
            Progress?.Report(new("Директория является дочерней для одной из добавленных", ReportLevel.Error));
            return;
        }

        if (dir.IsParentDirectoryOfAny(dirs))
        {
            Paths = new(dirs.Where(d => !d.IsChildDirectoryOf(dir))
                .Select(d => d.FullName).ToList());
            Progress?.Report(new("Директории объединены"));
        }

        var ignoreDirs = IgnorePaths.Select(d => new DirectoryInfo(d)).ToList();
        if (dir.IsChildDirectoryOfAny(ignoreDirs))
        {
            IgnorePaths = new(ignoreDirs.Where(d => !dir.IsChildDirectoryOf(d))
                .Select(d => d.FullName).ToList());
            Progress?.Report(new("Конфликтующие директории для игнорирования удалены"));
        }

        Paths.Add(path);
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
        if (ignoreDir.IsChildDirectoryOfAny(ignoreDirs))
        {
            Progress?.Report(new("Директория является дочерней для одной из добавленных", ReportLevel.Error));
            return;
        }

        if (ignoreDir.IsParentDirectoryOfAny(ignoreDirs))
        {
            IgnorePaths = new(ignoreDirs.Where(d => !d.IsChildDirectoryOf(ignoreDir))
                .Select(d => d.FullName).ToList());
            Progress?.Report(new("Директории объединены"));
        }

        var dirs = Paths.Select(d => new DirectoryInfo(d)).ToList();
        if (ignoreDir.IsParentDirectoryOfAny(dirs))
        {
            Paths = new(dirs.Where(d => !d.IsChildDirectoryOf(ignoreDir))
                .Select(d => d.FullName).ToList());
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
        Paths.Remove(path);
        SelectedPath = null;
    }

    private bool CanDeletePath() => SelectedPath is not null;

    [RelayCommand(CanExecute = nameof(CanDeleteIgnorePath))]
    private void DeleteIgnorePath(string path)
    {
        IgnorePaths.Remove(path);
        SelectedIgnorePath = null;
    }

    private bool CanDeleteIgnorePath() => SelectedIgnorePath is not null;

    [RelayCommand]
    private async Task PreparePaths()
    {
        if (RepeatAnalyze) PathsToAnalyze = Paths.ToList();
        else
        {
            var analyzedPaths = (await DirectoryDatabase.GetDirectoriesAsync(
                    dir => Paths.Contains(dir.DirectoryPath)))
                .Select(dir => dir.DirectoryPath).ToList();
            PathsToAnalyze = Paths.Where(path => !analyzedPaths.Contains(path)).ToList();
        }
    }

    partial void OnPathsChanged(ObservableCollection<string> value)
    {
        IsCanAnalyze();
        value.CollectionChanged += (_, _) => IsCanAnalyze();
    }

    private void IsCanAnalyze() => CanAnalyze = Paths.Count > 0;
}