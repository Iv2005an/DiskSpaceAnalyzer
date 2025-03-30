using CommunityToolkit.Maui;
using DiskSpaceAnalyzerMauiApp.Resources.ViewModels;
using DiskSpaceAnalyzerMauiApp.Resources.Views;
using Microsoft.Extensions.Logging;

namespace DiskSpaceAnalyzerMauiApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit();
            builder.Services.AddSingleton<AnalyzeSettingsPage>();
            builder.Services.AddSingleton<AnalyzeSettingsViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}