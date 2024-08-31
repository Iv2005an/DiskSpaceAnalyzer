using System.Text.RegularExpressions;

namespace DiskSpaceAnalyzerConsole;

internal static partial class Regexs
{
    [GeneratedRegex(@"[a-z][A-Z]")]
    public static partial Regex WordTranzitionRegex();
}
