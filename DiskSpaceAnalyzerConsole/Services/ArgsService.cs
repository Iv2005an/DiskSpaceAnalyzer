using DiskSpaceAnalyzerConsole.Models;
using static DiskSpaceAnalyzerConsole.Constants;
using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerConsole.Services;

public static class ArgsService
{
    public static Command? Parse(string[] args)
    {
        if (args.Length == 0) return new Command(Commands.Help);
        Commands? c = Command.GetCommand(args[0].ToLower());
        if (c is null)
        {
            PrintService.PrintErrorMessage($"Invalid command: `{args[0]}`\n");
            return null;
        }

        Commands command = (Commands)c;
        bool isRepeat = false;
        bool isIgnore = false;
        bool isAll = false;
        bool isAllCategories = false;
        List<FileTypes> categories = [];
        List<string> sourcePaths = [];
        List<string> ignorePaths = [];
        string pathToSave = "";

        bool commandWithParameters = commandsWithParameters.Contains(command);
        foreach (string arg in args[1..])
        {
            if (arg.StartsWith('-'))
            {
                if (commandWithParameters)
                {
                    switch (Command.GetParameter(command, arg))
                    {
                        case Parameters.Repeat:
                            isRepeat = true;
                            continue;
                        case Parameters.Ignore:
                            isIgnore = true;
                            continue;
                    }
                }
                PrintService.PrintErrorMessage($"Invalid named parameter: `{arg}`\n");
                return null;
            }
            else
            {
                if (commandWithParameters)
                {
                    if (command == Commands.Sort)
                    {
                        FileTypes? category = null;
                        foreach (FileTypes fileType in Enum.GetValues<FileTypes>())
                        {
                            if (string.Equals(fileType.ToString(), arg, StringComparison.CurrentCultureIgnoreCase))
                            {
                                category = fileType;
                                break;
                            }
                        }
                        if (category is not null)
                        {
                            if (isAllCategories) continue;
                            categories.Add((FileTypes)category);
                            continue;
                        }
                    }
                    string fullPath = Path.GetFullPath(arg);
                    if (Directory.Exists(fullPath))
                    {
                        if (isIgnore) ignorePaths.Add(fullPath);
                        else sourcePaths.Add(fullPath);
                        continue;
                    }
                }
                PrintService.PrintErrorMessage($"Invalid positional parameter: `{arg}`\n");
                return null;
            }
        }
        if (command == Commands.Sort)
        {
            if (sourcePaths.Count == 0)
            {
                PrintService.PrintErrorMessage($"Path to saving directory is required");
                return null;
            }
            else if (sourcePaths.Count == 1)
            {
                PrintService.PrintErrorMessage($"Source path is required");
                return null;
            }
            if (categories.Count == 0)
            {
                isAllCategories = true;
                FileTypes[] allCategories = Enum.GetValues<FileTypes>();
                categories = [.. allCategories[..(allCategories.Length - 1)]];
            }
            pathToSave = sourcePaths.Last();
            sourcePaths = sourcePaths[..(sourcePaths.Count - 1)];
        }
        else if (commandWithParameters && sourcePaths.Count == 0) isAll = true;
        return new Command(command, isRepeat, isAll, isAllCategories, sourcePaths, ignorePaths, categories, pathToSave);
    }
}