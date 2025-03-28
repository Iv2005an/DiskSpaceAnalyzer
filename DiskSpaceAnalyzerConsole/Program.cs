using System.CommandLine;
using System.Text.RegularExpressions;
using DiskSpaceAnalyzerConsole.Services;
using static DiskSpaceAnalyzerLib.Models.Category;

Command categoriesCommand = new("categories", "Show available categories and their extensions");
categoriesCommand.AddAlias("c");
categoriesCommand.SetHandler(ProgramService.Categories);

Option<List<DirectoryInfo>> dirsOption = new(["--dirs", "-d"], "Source directories")
{
    IsRequired = true,
    AllowMultipleArgumentsPerToken = true,
};
Option<List<DirectoryInfo>> ignoreDirsOption = new(["--ignore_dirs", "-id"], "Ignore directories")
{
    AllowMultipleArgumentsPerToken = true,
};
Option<bool> repeatOption = new(["--repeat", "-r"], "Repeat analysis of the analyzed directories");
Command analyzeCommand = new("analyze", "Run a directory analysis")
{
    dirsOption,
    ignoreDirsOption,
    repeatOption,
};
analyzeCommand.AddAlias("a");
analyzeCommand.SetHandler(
    ProgramService.Analyze,
    dirsOption, ignoreDirsOption, repeatOption);

Command infoCommand = new("info", "Show information about the analyzed directories")
{
    dirsOption,
    ignoreDirsOption,
};
infoCommand.AddAlias("i");
infoCommand.SetHandler(ProgramService.Info, dirsOption, ignoreDirsOption);


Argument<Regex> regexArgument = new("filename_regex", "User rule file name regex");
Argument<string> pathArgument = new("output_path", "User rule output path");
Command createRuleCommand = new("create", "Create user rule") { regexArgument, pathArgument };
createRuleCommand.AddAlias("c");
createRuleCommand.SetHandler(ProgramService.CreateRule, regexArgument, pathArgument);
Argument<int> ruleNumberArgument = new("rule_number", "User rule number");
Command deleteRuleCommand = new("delete", "Delete user rule") { ruleNumberArgument };
deleteRuleCommand.AddAlias("d");
deleteRuleCommand.SetHandler(ProgramService.DeleteRule, ruleNumberArgument);
Command onRuleCommand = new("on", "Turn on user rule") { ruleNumberArgument };
onRuleCommand.SetHandler(ProgramService.OnRule, ruleNumberArgument);
Command offRuleCommand = new("off", "Turn off user rule") { ruleNumberArgument };
offRuleCommand.SetHandler(ProgramService.OffRule, ruleNumberArgument);
Command rulesCommand = new("rules", "Show a list of the user rules")
{
    createRuleCommand,
    deleteRuleCommand,
    onRuleCommand,
    offRuleCommand,
};
rulesCommand.AddAlias("r");
offRuleCommand.SetHandler(ProgramService.Rules);

Option<List<Categories>> categoriesOption = new(["--categories", "-c"], "Categories for organize")
{
    IsRequired = true,
    AllowMultipleArgumentsPerToken = true,
};
Option<DirectoryInfo> outputDirOption = new(["--output", "-o"], "Output directory for save organized data")
{
    IsRequired = true,
};
Command organizeCommand = new("organize", "Run a directory organize")
{
    categoriesOption,
    dirsOption,
    ignoreDirsOption,
    outputDirOption,
    repeatOption,
};
organizeCommand.AddAlias("o");
organizeCommand.SetHandler(
    ProgramService.Organize,
    categoriesOption, dirsOption, ignoreDirsOption, outputDirOption, repeatOption);

RootCommand rootCommand = new("App for organize your disk space")
{
    categoriesCommand,
    analyzeCommand,
    infoCommand,
    rulesCommand,
    organizeCommand,
};
await rootCommand.InvokeAsync(args);
