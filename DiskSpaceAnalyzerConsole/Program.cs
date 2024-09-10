using DiskSpaceAnalyzerConsole.Models;
using DiskSpaceAnalyzerConsole.Services;
using static DiskSpaceAnalyzerConsole.Constants;

Command? command = ArgsService.Parse(args);
if (command is null) return;

switch (command.CommandName)
{
    case Commands.Help:
        ProgramService.Help();
        break;
    case Commands.AnalyzedDirs:
        await ProgramService.AnalyzedDirs();
        break;
    case Commands.Categories:
        ProgramService.Categories();
        break;
    case Commands.Analyze:
        await ProgramService.Analyze(command);
        break;
    case Commands.Info:
        await ProgramService.Info(command);
        break;
}
