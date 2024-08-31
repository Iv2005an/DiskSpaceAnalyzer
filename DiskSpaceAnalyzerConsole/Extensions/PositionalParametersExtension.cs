using static DiskSpaceAnalyzerConsole.Constants;

namespace DiskSpaceAnalyzerConsole.Extensions;

internal static class PositionalParametersExtension
{
    public static string GetName(this PositionalParameters parameter) =>
        positionalParameters[(int)parameter];
    public static string GetDescription(this PositionalParameters parameter) =>
        positionalParametersDescriptions[(int)parameter];
}
