using DiskSpaceAnalyzerConsole.Models;
using static DiskSpaceAnalyzerConsole.Constants;

namespace DiskSpaceAnalyzerTest
{
    public class CommandTest
    {
        [Fact]
        public void TestGetCommand()
        {
            Assert.Equal(Commands.Help, Command.GetCommand("help"));
            Assert.Equal(Commands.Help, Command.GetCommand("h"));
            Assert.Equal(Commands.Help, Command.GetCommand("HeLp"));
            Assert.Equal(Commands.Help, Command.GetCommand("H"));

            Assert.Equal(Commands.AnalyzedDirs, Command.GetCommand("analyzed_dirs"));
            Assert.Equal(Commands.AnalyzedDirs, Command.GetCommand("ad"));
            Assert.Equal(Commands.AnalyzedDirs, Command.GetCommand("AnAlYzEd_DiRs"));
            Assert.Equal(Commands.AnalyzedDirs, Command.GetCommand("AD"));

            Assert.Null(Command.GetCommand("dasdasd_dsa"));
        }
    }
}
