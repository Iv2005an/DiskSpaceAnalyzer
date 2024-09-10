namespace DiskSpaceAnalyzerConsole;

public static class Constants
{
    public enum Commands
    {
        Help,
        AnalyzedDirs,
        Categories,
        Analyze,
        Info,
        Sort,
    }
    public enum Parameters
    {
        All,
        Repeat,
        AllCategories,
    }
    public enum PositionalParameters
    {
        Paths,
        Categories,
        PathToSave,
    }

    public static readonly Commands[] commandsWithParameters =
    [
        Commands.Analyze,
        Commands.Info,
        Commands.Sort,
    ];
    public static readonly string[] commandsDescriptions =
    [
        "show a help message(DEFAULT)",
        "show a list of the analyzed directories",
        "show available file categories and their extensions",
        "run a directory analysis",
        "show information about the analyzed directories",
        "sort the data and save it",
    ];
    public static readonly string[] parametersDescriptions =
    [
        "use analyzed directories(DEFAULT)",
        "re-analysis of the analyzed directories",
        "use all categories(DEFAULT)",
    ];
    public static readonly string[] positionalParameters =
    [
        "<path_0> <path_1> ... <path_n>",
        "<category_1 category_2 ... category_n>",
        "<path_to_save>",
    ];
    public static readonly string[] positionalParametersDescriptions =
    [
        "paths to source directories, herewith --all[-a] is not DEFAULT",
        "select categories for sorting.\n    Available categories:",
        "path to directory for saving sorted data(REQUIRED)",
    ];
}
