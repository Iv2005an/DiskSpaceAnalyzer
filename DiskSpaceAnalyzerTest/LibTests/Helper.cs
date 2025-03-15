using DiskSpaceAnalyzerLib.Databases;

namespace DiskSpaceAnalyzerTest.LibTests;

public class Helper
{
    public static DirectoryInfo GetSolutionPath()
    {
        DirectoryInfo directory = new(Environment.CurrentDirectory);
        while (directory.Parent is not null)
        {
            if (directory.GetFiles("*.sln").Length != 0) return directory;
            directory = directory.Parent;
        }
        return new(Environment.CurrentDirectory);
    }

    public static DirectoryInfo[] Prepare(string testName, bool clear = true)
    {
        string solutionPath = GetSolutionPath().FullName;
        DirectoryInfo sourceDir = new(Path.Combine(solutionPath, "TestDir"));
        DirectoryInfo outputDir = new(Path.Combine(solutionPath, "OutputTestDir", testName));
        DirectoryInfo databasesDir = new(Path.Combine(outputDir.FullName, "_databases"));

        outputDir.Create();
        if (clear) outputDir.Delete(true);
        databasesDir.Create();

        Database.DatabasePath = databasesDir.FullName;

        return [sourceDir, outputDir, databasesDir];
    }
}