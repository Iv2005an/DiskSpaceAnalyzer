namespace DiskSpaceAnalyzerConsole;

public static class Constants
{
    public enum Commands
    {
        Help,
        Categories,
        Analyze,
        Info,
        Sort,
    }
    public enum Parameters
    {
        Repeat,
        Ignore,
    }
    public enum PositionalParameters
    {
        SourcePaths,
        IgnorePaths,
        Categories,
        SavePath,
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
        "show available file categories and their extensions",
        "run a directory analysis",
        "show information about the analyzed directories",
        "sort the data and save it",
    ];
    public static readonly string[] parametersDescriptions =
    [
        "re-analysis of the analyzed directories",
        "next paths after this flag will be ignored"
    ];
    public static readonly string[] positionalParameters =
    [
        "<source_path_0> <source_path_1> ... <source_path_n>",
        "<ignore_path_0> <ignore_path_1> ... <ignore_path_n>",
        "<save_path>",
        "<category_1 category_2 ... category_n>",
    ];
    public static readonly string[] positionalParametersDescriptions =
    [
        "paths to source directories(REQUIRED)",
        "paths to directories for ignoring",
        "last path before ignore flag will be defined as path to directory for saving sorted data(REQUIRED)",
        "select categories for sorting.(By default all categories)\n    Available categories:",
    ];
}
