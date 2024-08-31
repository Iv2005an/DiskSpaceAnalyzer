using DiskSpaceAnalyzerConsole.Models;
using DiskSpaceAnalyzerConsole.Services;
using DiskSpaceAnalyzerLib.Services;
using static DiskSpaceAnalyzerConsole.Constants;
using static DiskSpaceAnalyzerLib.Constants;

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
            foreach (string path in directoriesPath)
                analyzed_dirs += $"\n - {path}";
            PrintService.PrintWarningMessage(analyzed_dirs);
        }
        else PrintService.PrintWarningMessage("Analyzed directories are missing, run `analyze` command");
        break;
    case Commands.Categories:
        PrintService.PrintSuccessMessage("Available categories for analysis:\n");
        PrintService.PrintCategoryInfo("Raster", RasterExtensions);
        PrintService.PrintCategoryInfo("Vector", VectorExtensions);
        PrintService.PrintCategoryInfo("Text", TextExtensions);
        PrintService.PrintCategoryInfo("Audio", AudioExtensions);
        PrintService.PrintCategoryInfo("Video", VideoExtensions);
        PrintService.PrintCategoryInfo("EBook", EBookExtensions);
        PrintService.PrintCategoryInfo("CAD", CadExtensions);
        PrintService.PrintCategoryInfo("Presentation", PresentationExtensions);
        PrintService.PrintCategoryInfo("Spreadsheet", SpreadsheetExtensions);
        PrintService.PrintCategoryInfo("Database", DatabaseExtensions);
        PrintService.PrintCategoryInfo("Archive", ArchiveExtensions);
        PrintService.PrintCategoryInfo("Web", WebExtensions);
        PrintService.PrintCategoryInfo("Developer", DeveloperExtensions);
        PrintService.PrintCategoryInfo("System", SystemExtensions);
        PrintService.PrintCategoryInfo("Executables", ExecutablesExtensions);
        PrintService.PrintCategoryInfo("Settings", SettingsExtensions);
        break;
}
