using DiskSpaceAnalyzerMauiApp.Resources.ViewModels;

namespace DiskSpaceAnalyzerMauiApp.Resources.Views;

public partial class AnalyzePage : ContentPage
{
    private readonly AnalyzeViewModel _viewModel;

    public AnalyzePage(AnalyzeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    private async void GoToOrganize_OnClicked(object? sender, EventArgs e)
    {
        var parameters = new ShellNavigationQueryParameters
        {
            { "paths", _viewModel.InputPaths },
            { "ignorePaths", _viewModel.InputIgnorePaths }
        };
        await Shell.Current.GoToAsync("//InfoPage", parameters);
    }

    private async void GoBack_OnClicked(object? sender, EventArgs e)
    {
        if (!await DisplayAlert("Внимание", "Вернуться назад?", "Да", "Отмена")) return;
        await Shell.Current.GoToAsync("//AnalyzeSettingsPage");
    }
}