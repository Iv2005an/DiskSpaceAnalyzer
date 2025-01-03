using DiskSpaceAnalyzerConsole.Models;
using DiskSpaceAnalyzerConsole.Services;
using static DiskSpaceAnalyzerConsole.Constants;
using static DiskSpaceAnalyzerLib.Constants;

namespace DiskSpaceAnalyzerTest
{
    public class ArgsServiceTest
    {
        [Fact]
        public void TestParseDefaultCommand() => Assert.Equivalent(new Command(Commands.Help), ArgsService.Parse([]));

        [Fact]
        public void TestParseCommandsWithoutParameters()
        {
            Assert.Equivalent(new Command(Commands.Help), ArgsService.Parse(["help"]), true);
            Assert.Equivalent(new Command(Commands.Help), ArgsService.Parse(["h"]), true);

            Assert.Equivalent(new Command(Commands.AnalyzedDirs), ArgsService.Parse(["analyzed_dirs"]), true);
            Assert.Equivalent(new Command(Commands.AnalyzedDirs), ArgsService.Parse(["ad"]), true);

            Assert.Equivalent(new Command(Commands.Categories), ArgsService.Parse(["categories"]), true);
            Assert.Equivalent(new Command(Commands.Categories), ArgsService.Parse(["c"]), true);

            Assert.Null(ArgsService.Parse(["helps"]));
            Assert.Null(ArgsService.Parse(["help", "-r"]));
        }

        [Fact]
        public void TestParseCommandsWithParameters()
        {
            Assert.Equivalent(new Command(Commands.Analyze, isAll: true), ArgsService.Parse(["analyze"]), true);
            Assert.Equivalent(new Command(Commands.Analyze, isAll: true), ArgsService.Parse(["a"]), true);
            Assert.Equivalent(
                new Command(Commands.Analyze, true, true, paths: ["D:\\"]),
                ArgsService.Parse(["a", "-r", "-a", "D:\\"]), true);

            Assert.Equivalent(new Command(Commands.Percentages, isAll: true), ArgsService.Parse(["percentages"]), true);
            Assert.Equivalent(new Command(Commands.Percentages, isAll: true), ArgsService.Parse(["p"]), true);
            Assert.Equivalent(
                new Command(Commands.Percentages, true, true, paths: ["D:\\"]),
                ArgsService.Parse(["p", "-r", "-a", "D:\\"]), true);

            Assert.Equivalent(new Command(Commands.Sort, isAll: true, isAllCategories: true, paths: ["D:\\"]),
                ArgsService.Parse(["sort", "D:\\"]), true);
            Assert.Equivalent(new Command(Commands.Sort, isAll: true, isAllCategories: true, paths: ["D:\\"]),
                ArgsService.Parse(["s", "D:\\"]), true);
            Assert.Equivalent(
                new Command(Commands.Sort, true, true, true, paths: ["D:\\"]),
                ArgsService.Parse(["s", "-r", "-a", "-ac", "Raster", "vEcToR", "D:\\"]), true);
            Assert.Equivalent(
                new Command(Commands.Sort, true, true,
                paths: ["D:\\"], categories: [FileTypes.Raster, FileTypes.Vector]),
                ArgsService.Parse(["s", "-r", "-a", "Raster", "vEcToR", "D:\\"]), true);
            Assert.Null(ArgsService.Parse(["s"]));
        }
    }
}
