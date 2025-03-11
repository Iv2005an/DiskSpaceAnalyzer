using DiskSpaceAnalyzerLib.Databases;
using DiskSpaceAnalyzerLib.Services;

namespace DiskSpaceAnalyzerTest.LibTests

{
    public class DirectoryServiceTest
    {
        static DirectoryInfo GetSolutionPath()
        {
            DirectoryInfo directory = new(Environment.CurrentDirectory);
            while (directory.Parent is not null)
            {
                if (directory.GetFiles("*.sln").Length != 0) return directory;
                directory = directory.Parent;
            }
            return new(Environment.CurrentDirectory);
        }

        [Fact]
        public async Task AnalyzeTest()
        {
            DirectoryInfo sourceDir = new(Path.Combine(GetSolutionPath().FullName, "TestDir"));
            DirectoryInfo outputDir = new(Path.Combine(GetSolutionPath().FullName, "OutputTestDir"));
            DirectoryInfo dbsDir = new(Path.Combine(outputDir.FullName, "dbs"));
            dbsDir.Create();

            Database.DatabasePath = Path.Combine(outputDir.FullName, "dbs");
            Database.DatabaseFilename = "1.db3";
            await DirectoryService.Analyze([sourceDir]);

            Database.DatabasePath = Path.Combine(outputDir.FullName, "dbs");
            Database.DatabaseFilename = "2.db3";
            await DirectoryService.Analyze([sourceDir], [new(Path.Combine(sourceDir.FullName, "Смешарики. Выпуск 1"))]);
        }
    }
}