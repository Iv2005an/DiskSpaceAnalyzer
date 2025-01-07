using DiskSpaceAnalyzerLib.Extensions;
using DiskSpaceAnalyzerLib.Models;
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
    public static void PrintAnalyzedCategoriesInfo(List<CategoryInfo> categoriesInfo)
    {
        foreach (CategoryInfo categoryInfo in categoriesInfo)
            PrintWarningMessage(
                $"{categoryInfo.Category}: {categoryInfo.FilesCount} ({categoryInfo.Percentages:P2})\n");
    }
    public static void PrintCompletedMessage() => PrintSuccessMessage("COMPLETED\n");
}
