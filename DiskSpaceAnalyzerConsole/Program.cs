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
        PrintService.PrintWarningMessage("""
            - `help[h]` - show help message
            - `analyzed_dirs[ad]` - show list of analyzed directories
            - `categories[c]` - show available file categories and their extensions
            - `analyze[a]` - run directory analysis 
              - `--repeat[-r]` - repeat analysis if directory has been analyzed
              - `--all[-a]` - check analysis of analyzed directories(DEFAULT)
              - `<path_0> <path_1> ... <path_n>` - paths to directories for analysis, `--all[-a]` is not DEFAULT
            - `percentages[p]` - show percentage of categories of analyzed directories
              - `--repeat[-r]` - repeat analysis if directory has been analyzed
              - `--all[-a]` - use analyzed directories(DEFAULT)
              - `<path_0> <path_1> ... <path_n>` - paths to directories for analysis and showing percentage, `--all[-a]` is not DEFAULT
            - `sort[s]` - create directory with sorted data by all categories
              - `--repeat[-r]` - repeat analysis if directory has been analyzed
              - `--all[-a]` - use analyzed directories(DEFAULT)
              - `<path_0> <path_1> ... <path_n>` - paths to directories for analysis and sorting, `--all[-a]` is not DEFAULT
              - `<path_to_save>` - last path is path to save sorted data(REQUIRED)
            - `sort_categories[sc]` - create sorted data by selected categories
              - `--repeat[-r]` - repeat analysis if directory has been analyzed
              - `<category_1 category_2 ... category_n>` - selected categories to sort, available categories:
                - RasterImage
                - VectorImage
                - Text
                - Audio
                - Video
                - EBook
                - CAD
                - Presentation
                - Spreadsheet
                - Database
                - Archive
                - Web
                - Developer
                - System
                - Executables
                - Settings
                - Other
              - `--all[-a]` - use analyzed directories(DEFAULT)
              - `<path_0> <path_1> ... <path_n>` - paths to directories for analysis and sorting, `--all[-a]` is not DEFAULT
              -  `<path_to_save>` - last path is path to save sorted data(REQUIRED)
              For more information: https://github.com/Iv2005an/DiskSpaceAnalyzer/
            """);
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
