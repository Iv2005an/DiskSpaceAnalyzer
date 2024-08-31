using System.Text.RegularExpressions;
using static DiskSpaceAnalyzerConsole.Constants;
using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerConsole.Models;

public partial class Command(Commands commandName,
                             bool isRepeat = false,
                             bool isAll = false,
                             bool isAllCategories = false,
                             List<string>? paths = null,
                             List<FileTypes>? categories = null)
{
    public Commands CommandName => commandName;
    public bool IsRepeat => isRepeat;
    public bool IsAll => isAll;
    public bool IsAllCategories => isAllCategories;
    public List<string> Paths => paths ?? [];
    public List<FileTypes> Categories => categories ?? [];

    public static Commands? GetCommand(string commandName)
    {
        if (!string.IsNullOrEmpty(commandName))
        {
            foreach (Commands command in Enum.GetValues<Commands>())
            {
                string commandS = command.ToString();
                if (string.Equals(
                    WordTranzitionRegex().Replace(commandS, (m) => $"{m.Value[0]}_{m.Value[1]}"),
                    commandName,
                    StringComparison.CurrentCultureIgnoreCase)
                    || string.Equals(
                    string.Concat(commandS.Where(char.IsUpper)),
                    commandName,
                    StringComparison.CurrentCultureIgnoreCase)) return command;
            }
        }
        return null;
    }

    [GeneratedRegex("[a-z][A-Z]")]
    private static partial Regex WordTranzitionRegex();
}