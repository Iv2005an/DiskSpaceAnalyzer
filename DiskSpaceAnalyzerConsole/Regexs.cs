using System.Text.RegularExpressions;

namespace DiskSpaceAnalyzerConsole;

internal static partial class Regexps
{
    [GeneratedRegex(@"[a-z][A-Z]")]
    public static partial Regex WordTransitionRegex();
}
