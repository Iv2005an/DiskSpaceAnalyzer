using static DiskSpaceAnalyzerConsole.Constants;

namespace DiskSpaceAnalyzerConsole.Extensions;

internal static class CommandsExtension
{
    public static string GetName(this Commands command) =>
        Regexs.WordTranzitionRegex().Replace(
        $"{command}", m => $"{m.Value[0]}_{m.Value[1]}").ToLower();
    public static string GetShortName(this Commands command)
    {
        string name = command.GetName();
        string shortName = "";
        foreach (string word in name.Split('_')) shortName += $"{char.ToLower(word[0])}";
        return shortName;
    }
    public static string GetDescription(this Commands command) =>
        commandsDescriptions[(int)command];
    public static Parameters[] GetParameters(this Commands command) =>
        (int)command < 3
        ? [] : (int)command == 5
        ? Enum.GetValues<Parameters>() : [Parameters.All, Parameters.Repeat];
    public static PositionalParameters[] GetPositionalParameters(this Commands command) =>
        (int)command < 3
        ? [] : (int)command == 5
        ? Enum.GetValues<PositionalParameters>() : [PositionalParameters.Paths];
}
