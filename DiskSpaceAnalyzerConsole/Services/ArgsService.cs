using DiskSpaceAnalyzerConsole.Models;
using static DiskSpaceAnalyzerConsole.Constants;
using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerConsole.Services
{
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
            bool isAll = false;
            bool isAllCategories = false;
            List<FileTypes> categories = [];
            List<string> paths = [];

            bool commandWithParameters = COMMANDS_WITH_PARAMETERS.Contains(command);
            foreach (string arg in args[1..])
            {
                if (arg.StartsWith('-'))
                {
                    if (commandWithParameters)
                    {
                        if (AVAILABLE_PARAMETERS[0..2].Contains(arg))
                        {
                            isRepeat = true;
                            continue;
                        }
                        else if (AVAILABLE_PARAMETERS[2..4].Contains(arg))
                        {
                            isAll = true;
                            continue;
                        }
                        else if (command == Commands.Sort && AVAILABLE_PARAMETERS[4..6].Contains(arg))
                        {
                            isAllCategories = true;
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
                            paths.Add(fullPath);
                            continue;
                        }
                    }
                    PrintService.PrintErrorMessage($"Invalid positional parameter: `{arg}`\n");
                    return null;
                }
            }
            if (command == Commands.Sort)
            {
                if (paths.Count == 0)
                {
                    PrintService.PrintErrorMessage($"Path to saving directory is required");
                    return null;
                }
                else if (paths.Count == 1) isAll = true;
                if (categories.Count == 0) isAllCategories = true;
            }
            else if (commandWithParameters && paths.Count == 0) isAll = true;
            return new Command(command, isRepeat, isAll, isAllCategories, paths, categories);
        }
    }
}