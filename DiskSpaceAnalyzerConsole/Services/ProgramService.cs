using DiskSpaceAnalyzerLib.Services;

namespace DiskSpaceAnalyzerConsole.Services;

internal static class ProgramService
{
    public static void Help() => PrintService.PrintCommandsDescriptions();
    public static async Task AnalyzedDirs()
    {
        List<string> directoriesPath = await AnalyzedDirectoriesService.GetDirectoriesPaths();
        if (directoriesPath.Count > 0)
        {
            PrintService.PrintSuccessMessage("Analyzed directories:");
            string analyzed_dirs = "";
            foreach (string path in directoriesPath) analyzed_dirs += $"\n - {path}";
            PrintService.PrintWarningMessage(analyzed_dirs);
        }
        else PrintService.PrintWarningMessage("Analyzed directories are missing, run `analyze` command");
    }
    public static void Categories() => PrintService.PrintCategoriesInfo();
}
