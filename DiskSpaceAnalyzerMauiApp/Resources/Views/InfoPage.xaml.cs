using DiskSpaceAnalyzerMauiApp.Resources.ViewModels;

namespace DiskSpaceAnalyzerMauiApp.Resources.Views;

public partial class InfoPage : ContentPage
{
    private readonly InfoViewModel _viewModel;

    public InfoPage(InfoViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void RunOrganize_OnClicked(object? sender, EventArgs e)
    {
        var parameters = new ShellNavigationQueryParameters
        {
            { "paths", _viewModel.InputPaths },
            { "ignorePaths", _viewModel.InputIgnorePaths },
            { "outputDir", _viewModel.OutputPath },
            { "categoryInfos", _viewModel.SelectedCategories },
        };
        await Shell.Current.GoToAsync("//OrganizePage", parameters);
    }

    private async void GoAnalyze_OnClicked(object? sender, EventArgs e)
    {
        if (!await DisplayAlert("Внимание", "Повторить анализ?", "Да", "Отмена")) return;
        await Shell.Current.GoToAsync("//AnalyzeSettingsPage");
    }
}