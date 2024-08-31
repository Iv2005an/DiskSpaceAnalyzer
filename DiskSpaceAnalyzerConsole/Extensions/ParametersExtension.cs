using static DiskSpaceAnalyzerConsole.Constants;

namespace DiskSpaceAnalyzerConsole.Extensions;

internal static class ParametersExtension
{
    public static string GetName(this Parameters parameter) =>
        Regexs.WordTranzitionRegex().Replace(
        $"--{parameter}", m => $"{m.Value[0]}_{m.Value[1]}").ToLower();
    public static string GetShortName(this Parameters parameter)
    {
        string name = parameter.GetName()[2..];
        string shortName = "-";
        foreach (string word in name.Split('_')) shortName += $"{char.ToLower(word[0])}";
        return shortName;
    }
    public static string GetDescription(this Parameters parameter) =>
        parametersDescriptions[(int)parameter];
}
