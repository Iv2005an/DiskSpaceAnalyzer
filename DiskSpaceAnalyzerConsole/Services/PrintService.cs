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

    public static void PrintCategories()
    {
        PrintSuccessMessage("Available categories for analysis:\n");
        var categories = Enum.GetValues<FileTypes>();
        foreach (FileTypes category in categories[..(categories.Length - 2)])
        {
            PrintInfoMessage($"\n{category}:");
            string s = "";
            foreach (string extension in category.GetExtensions())
                s += $" {extension}";
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