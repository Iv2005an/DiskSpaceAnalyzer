using CommunityToolkit.Maui;
using DiskSpaceAnalyzerLib.Databases;
using DiskSpaceAnalyzerMauiApp.Resources.ViewModels;
using DiskSpaceAnalyzerMauiApp.Resources.Views;
using Microsoft.Extensions.Logging;

namespace DiskSpaceAnalyzerMauiApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Database.DatabasePath = FileSystem.AppDataDirectory;
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit();
            builder.Services.AddSingleton<AnalyzeSettingsPage>();
            builder.Services.AddSingleton<AnalyzeSettingsViewModel>();
            builder.Services.AddSingleton<AnalyzePage>();
            builder.Services.AddSingleton<AnalyzeViewModel>();
            builder.Services.AddSingleton<InfoPage>();
            builder.Services.AddSingleton<InfoViewModel>();
            builder.Services.AddSingleton<OrganizePage>();
            builder.Services.AddSingleton<OrganizeViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}