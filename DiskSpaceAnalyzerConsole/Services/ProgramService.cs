using DiskSpaceAnalyzerConsole.Models;
﻿using DiskSpaceAnalyzerLib.Services;

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
    public static async Task Analyze(Command command)
    {
        if (command.IsAll)
        {
            command.SourcePaths.AddRange(await AnalyzedDirectoriesService.GetDirectoriesPaths());
            PrintService.PrintInfoMessage($"Start analysis\n");
        }
        foreach (string path in command.SourcePaths)
        {
            if (!command.IsAll) PrintService.PrintInfoMessage($"Start analysis: {path}\n");
            await AnalyzedDirectoriesService.Analyze(
                path, command.IsRepeat,
                new Progress<string>((path) => PrintService.PrintWarningMessage($"{path}\n")));
            if (!command.IsAll) PrintService.PrintCompletedMessage();
        }
        if (command.IsAll) PrintService.PrintCompletedMessage();
    }
}
