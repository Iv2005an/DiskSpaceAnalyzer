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

    public static void PrintCategoryInfo(string name, string[] extensions)
    {
        PrintInfoMessage($"\n{name}:");
        string s = "";
        for (int i = 0; i < extensions.Length; i++)
        {
            string extension = extensions[i];
            s += $"{(i % 10 == 0 ? "\n " : ' ')}{extension}";
        }
        PrintWarningMessage($"{s}\n");
    }
}
