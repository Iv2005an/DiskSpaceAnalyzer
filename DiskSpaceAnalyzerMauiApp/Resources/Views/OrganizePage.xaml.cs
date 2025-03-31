using DiskSpaceAnalyzerMauiApp.Resources.ViewModels;

namespace DiskSpaceAnalyzerMauiApp.Resources.Views;

public partial class OrganizePage : ContentPage
{
    private readonly OrganizeViewModel _viewModel;

    public OrganizePage(OrganizeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    private async void Complete_OnClicked(object? sender, EventArgs e)
    {
        var parameters = new ShellNavigationQueryParameters
        {
            { "paths", _viewModel.InputPaths },
            { "ignorePaths", _viewModel.InputIgnorePaths }
        };
        await Shell.Current.GoToAsync("//InfoPage", parameters);
    }
}