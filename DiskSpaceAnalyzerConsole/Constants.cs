namespace DiskSpaceAnalyzerConsole
{
    public static partial class Constants
    {
        public enum Commands
        {
            Help,
            AnalyzedDirs,
            Categories,
            Analyze,
            Percentages,
            Sort,
        }
        public static readonly List<Commands> COMMANDS_WITH_PARAMETERS =
        [
            Commands.Analyze,
            Commands.Percentages,
            Commands.Sort
        ];
        public static readonly string[] AVAILABLE_PARAMETERS = ["--repeat",         "-r",
                                                                "--all",            "-a",
                                                                "--all_categories", "-ac"];
    }
}
