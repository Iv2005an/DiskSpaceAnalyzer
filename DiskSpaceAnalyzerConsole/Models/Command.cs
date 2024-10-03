using DiskSpaceAnalyzerConsole.Extensions;
using static DiskSpaceAnalyzerConsole.Constants;
using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerConsole.Models;

public class Command(Commands commandName,
                             bool isRepeat = false,
                             List<string>? sourcePaths = null,
                             string? pathToSave = null,
                             List<string>? ignorePaths = null,
                             List<FileTypes>? categories = null)
{
    public Commands CommandName => commandName;
    public bool IsRepeat => isRepeat;
    public List<string> SourcePaths => sourcePaths ?? [];
    public string PathToSave => pathToSave ?? "";
    public List<string> IgnorePaths => ignorePaths ?? [];
    public List<FileTypes> Categories => categories ?? [];

    public static Commands? GetCommand(string commandName)
    {
        if (!string.IsNullOrEmpty(commandName))
        {
            foreach (Commands command in Enum.GetValues<Commands>())
            {
                if (string.Equals(
                    command.GetName(),
                    commandName,
                    StringComparison.CurrentCultureIgnoreCase)
                    || string.Equals(
                    command.GetShortName(),
                    commandName,
                    StringComparison.CurrentCultureIgnoreCase)) return command;
            }
        }
        return null;
    }
    public static Parameters? GetParameter(Commands command, string parameterName)
    {
        if (!string.IsNullOrEmpty(parameterName))
        {
            Parameters[] availableParameters = command.GetParameters();
            foreach (Parameters parameter in Enum.GetValues<Parameters>())
            {
                if (availableParameters.Contains(parameter)
                    && string.Equals(
                    parameter.GetName(),
                    parameterName,
                    StringComparison.CurrentCultureIgnoreCase)
                    || string.Equals(
                    parameter.GetShortName(),
                    parameterName,
                    StringComparison.CurrentCultureIgnoreCase)) return parameter;
            }
        }
        return null;
    }
}