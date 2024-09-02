using DiskSpaceAnalyzerConsole.Extensions;
using DiskSpaceAnalyzerLib.Extensions;
using static DiskSpaceAnalyzerConsole.Constants;
using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerConsole.Services;

internal static class PrintService
{
    static void PrintMessage(string message, ConsoleColor foregroundColor)
    {
        ConsoleColor baseColor = Console.ForegroundColor;
        Console.ForegroundColor = foregroundColor;
        Console.Write(message);
        Console.ForegroundColor = baseColor;
    }
    public static void PrintInfoMessage(string message) => PrintMessage(message, ConsoleColor.Blue);
    public static void PrintSuccessMessage(string message) => PrintMessage(message, ConsoleColor.Green);
    public static void PrintWarningMessage(string message) => PrintMessage(message, ConsoleColor.Yellow);
    public static void PrintErrorMessage(string message) => PrintMessage(message, ConsoleColor.Red);

    public static void PrintCategoriesInfo()
    {
        PrintSuccessMessage("Available categories for analysis:\n");
        foreach (FileTypes category in Enum.GetValues<FileTypes>())
        {
            if (category != FileTypes.Other && category != FileTypes.Error)
            {
                PrintInfoMessage($"\n{category}:");
                string s = "";
                string[] extensions = category.GetExtensions();
                for (int i = 0; i < extensions.Length; i++)
                {
                    string extension = extensions[i];
                    s += $"{(i % 10 == 0 ? "\n " : ' ')}{extension}";
                }
                PrintWarningMessage($"{s}\n");
            }
        }
    }
    public static void PrintCommandsDescriptions()
    {
        foreach (Commands command in Enum.GetValues<Commands>())
        {
            Parameters[] parameters = command.GetParameters();
            if (parameters.Length > 0) Console.WriteLine();
            PrintSuccessMessage($"`{command.GetName()}");
            Console.Write("[");
            PrintSuccessMessage($"{command.GetShortName()}");
            Console.Write("]");
            PrintSuccessMessage("`");
            PrintWarningMessage($" - {command.GetDescription()}\n");
            foreach (Parameters parameter in parameters)
            {
                PrintSuccessMessage($"  `{parameter.GetName()}");
                Console.Write("[");
                PrintSuccessMessage($"{parameter.GetShortName()}");
                Console.Write("]");
                PrintSuccessMessage("`");
                PrintWarningMessage($" - {parameter.GetDescription()}\n");
            }
            PositionalParameters[] positionalParameters = command.GetPositionalParameters();
            if (positionalParameters.Length > 0) Console.WriteLine();
            foreach (PositionalParameters parameter in positionalParameters)
            {
                PrintSuccessMessage($"  `{parameter.GetName()}`");
                PrintWarningMessage($" - {parameter.GetDescription()}");
                if ((int)parameter == 1)
                {
                    string s = "";
                    FileTypes[] categories = Enum.GetValues<FileTypes>();
                    for (int i = 0; i < categories.Length - 1; i++)
                        s += $"\n      - {categories[i]}";
                    PrintInfoMessage(s);
                }
                Console.WriteLine();
            }
        }
        PrintInfoMessage(
            "For more information: https://github.com/Iv2005an/DiskSpaceAnalyzer/");
    }
}
