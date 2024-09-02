using DiskSpaceAnalyzerConsole.Models;
using DiskSpaceAnalyzerConsole.Services;
using DiskSpaceAnalyzerLib.Services;
using static DiskSpaceAnalyzerConsole.Constants;

Command? command = ArgsService.Parse(args);
if (command is null) return;

switch (command.CommandName)
{
    case Commands.Help:
        PrintService.PrintCommandsDescriptions();
        break;
    case Commands.AnalyzedDirs:
        List<string> directoriesPath = await AnalyzedDirectoriesService.GetDirectoriesPaths();
        if (directoriesPath.Count > 0)
        {
            PrintService.PrintSuccessMessage("Analyzed directories:");
            string analyzed_dirs = "";
            foreach (string path in directoriesPath) analyzed_dirs += $"\n - {path}";
            PrintService.PrintWarningMessage(analyzed_dirs);
        }
        else PrintService.PrintWarningMessage("Analyzed directories are missing, run `analyze` command");
        break;
    case Commands.Categories:
        PrintService.PrintCategoriesInfo();
        break;
}
