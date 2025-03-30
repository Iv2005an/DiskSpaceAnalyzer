using DiskSpaceAnalyzerLib.Models;
using DiskSpaceAnalyzerMauiApp.Resources.ViewModels;

namespace DiskSpaceAnalyzerMauiApp.Resources.Views;

public partial class AnalyzeSettingsPage : ContentPage
{
    private readonly AnalyzeSettingsViewModel _viewModel;

    public AnalyzeSettingsPage(AnalyzeSettingsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _viewModel.Progress = ViewModelProgress;
        BindingContext = _viewModel;
    }

    private Progress<ProgressReport> ViewModelProgress => new(ViewModelProgressHandler);

    private void ViewModelProgressHandler(ProgressReport report)
    {
        var title = report.Level switch
        {
            ReportLevel.Info => "Информация",
            ReportLevel.Success => "Успех",
            ReportLevel.Warning => "Внимание",
            ReportLevel.Error => "Ошибка",
            _ => ""
        };

        DisplayAlert(title, report.Message, "OK");
    }

    private async void RunAnalyze_OnClicked(object? sender, EventArgs e)
    {
        if (!await DisplayAlert("Внимание", "Выполнить анализ?", "Выполнить", "Отмена")) return;
        await _viewModel.PreparePathsCommand.ExecuteAsync(null);
        var parameters = new ShellNavigationQueryParameters
        {
            { "paths", _viewModel.Paths.ToList() },
            { "ignorePaths", _viewModel.IgnorePaths.ToList() }
        };
        if (_viewModel.PathsToAnalyze.Count > 0)
        {
            parameters.Add("pathsToAnalyze", _viewModel.PathsToAnalyze);
            await Shell.Current.GoToAsync("//AnalyzePage", parameters);
        }
        else
        {
            await DisplayAlert("Внимание", "Анализ не требуется", "ОК");
            await Shell.Current.GoToAsync("//InfoPage", parameters);
        }
    }
}