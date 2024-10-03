using DiskSpaceAnalyzerConsole.Models;
using DiskSpaceAnalyzerLib.Databases;
using DiskSpaceAnalyzerLib.Models;
using DiskSpaceAnalyzerLib.Services;

namespace DiskSpaceAnalyzerConsole.Services;

internal static class ProgramService
{
    public static void Help() => PrintService.PrintCommandsDescriptions();
    public static void Categories() => PrintService.PrintCategoriesInfo();
    public static async Task Analyze(Command command)
    {
        foreach (string path in command.SourcePaths)
        {
            PrintService.PrintInfoMessage($"Start analysis: {path}\n");
            await AnalyzedDirectoriesService.Analyze(
                path, command.IsRepeat,
                new Progress<string>((path) => PrintService.PrintWarningMessage($"{path}\n")));
            PrintService.PrintCompletedMessage();
        }
    }
    public static async Task Info(Command command)
    {
        await Analyze(command);
        await AnalyzedFilesService.GetInfo(command.SourcePaths);
    }
    public static async Task Sort(Command command)
    {
        await Analyze(command);
        PrintService.PrintInfoMessage($"Start sorting\n");
        List<AnalyzedFile> filesToSort = [];
        foreach (string path in command.SourcePaths)
            filesToSort.AddRange(await AnalyzedFilesDatabase.GetFilesAsync(
                f => command.Categories.Contains(f.Type) && f.DirectoryPath.StartsWith(path)));
        CatalogService.Sort(
            filesToSort,
            command.PathToSave,
            onSuccessCopy: (f, i, np) => PrintService.PrintSuccessMessage(
                $"SUCCESS {i}/{filesToSort.Count} {(float)i / filesToSort.Count:P2} {np}\n"),
            onDuplicate: (f, i, p) => PrintService.PrintWarningMessage(
                $"DUPLICATE {i}/{filesToSort.Count} {(float)i / filesToSort.Count:P2} {p}\n"),
            onException: (f, i, p, e) => PrintService.PrintErrorMessage(
                $"ERROR {i}/{filesToSort.Count} {(float)i / filesToSort.Count:P2} {p} {e}\n"));
        PrintService.PrintCompletedMessage();
    }
}
