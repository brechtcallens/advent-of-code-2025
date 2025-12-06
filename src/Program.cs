using AdventOfCode2025.Solutions;
using System.CommandLine;

// Command: ./AdventOfCode2025
RootCommand rootCommand = [];

// Command: ./AdventOfCode2025 solve
Command solveCommand = new("solve", "Solve the advent of code puzzle for a specific day.");
rootCommand.Subcommands.Add(solveCommand);

// Command: ./AdventOfCode2025 solve <day>
Argument<int> messageArgument = new("day")
{
    Description = "Day of the puzzle to solve (e.g. 5)",
};
solveCommand.Arguments.Add(messageArgument);

// Command: ./AdventOfCode2025 solve <day> --part <part>
Option<DayPart> partOption = new("--part", "-p")
{
    Description = "Part of the puzzle to solve (defaults to both parts)",
    CustomParser = result =>
        result.Tokens[0].Value switch
        {
            "1" => DayPart.Part1,
            "2" => DayPart.Part2,
            _ => DayPart.Both
        }
};
partOption.AcceptOnlyFromAmong("1", "2", "both");
solveCommand.Options.Add(partOption);

// Command: ./AdventOfCode2025 solve <day> --example
Option<bool> exampleOption = new("--example", "-e")
{
    Description = "Solve example input instead of actual input",
    DefaultValueFactory = _ => false,
};
solveCommand.Options.Add(exampleOption);

// Action for the solve command
solveCommand.SetAction(parseResult =>
{
    var day = parseResult.GetRequiredValue(messageArgument);
    var part = parseResult.GetValue(partOption);
    var solveExample = parseResult.GetValue(exampleOption);
    new DaySolverFactory().GetSolver(day)?.Solve(part, solveExample);
});

// Go!
rootCommand.Parse(args).Invoke();
