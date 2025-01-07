using System.Text.RegularExpressions;
using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerConsole.Services;

internal static class ProgramService
{
    public static void Categories() => throw new NotImplementedException();

    public static async Task Analyze(
        List<DirectoryInfo> sourceDirs, List<DirectoryInfo> ignoreDirs,
        bool isRepeat)
    {
        throw new NotImplementedException();
    }

    public static async Task Info(
        List<DirectoryInfo> sourceDirs, List<DirectoryInfo> ignoreDirs)
    {
        throw new NotImplementedException();
    }

    public static async Task CreateRule(Regex regex, string path)
    {
        throw new NotImplementedException();
    }

    public static async Task DeleteRule(int ruleNumber)
    {
        throw new NotImplementedException();
    }

    public static async Task OnRule(int ruleNumber)
    {
        throw new NotImplementedException();
    }

    public static async Task OffRule(int ruleNumber)
    {
        throw new NotImplementedException();
    }

    public static async Task Rules()
    {
        throw new NotImplementedException();
    }

    public static async Task Organize(
        List<FileTypes> categories,
        List<DirectoryInfo> sourceDirs, List<DirectoryInfo> ignoreDirs,
        DirectoryInfo outputDir, bool isRepeat)
    {
        throw new NotImplementedException();
    }
}
