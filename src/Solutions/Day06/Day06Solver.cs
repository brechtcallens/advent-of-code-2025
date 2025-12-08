using System.Text.RegularExpressions;

namespace AdventOfCode2025.Solutions.Day06;

public class Day06Solver : BaseDaySolver
{
    public override int Day => 6;

    private static readonly string digitMatchRegexString = @"\d+";

    private readonly Regex digitGroupsRegex = new(digitMatchRegexString);

    protected override string SolvePart1(string[] input, bool isExample)
    {
        var numbersGroupedByLine = input[..^1]
            .Select(line =>
                digitGroupsRegex
                    .Matches(line)
                    .Select((match) => long.Parse(match.Value))
                    .ToArray())
            .ToArray();

        var operations = input[^1]
            .Where(c => c != ' ');

        var grandTotal = operations
            .Select((operation, exercise) =>
                CalculateExercise(operation, numbersGroupedByLine.Select((numbers) => numbers[exercise])))
            .Sum();

        return grandTotal.ToString();
    }

    protected override string SolvePart2(string[] input, bool isExample)
    {
        var operations = input[^1]
            .Where(c => c != ' ');

        var numberGrid = input[..^1]
            .Select(line => line.ToCharArray())
            .ToArray();

        var numbersGroupedByExercise = numberGrid[0]
            .Select((_, column) =>
                numberGrid.Select(line => line[column])
                    .Where(character => character != ' ')
                    .ToArray())
            .Aggregate(new List<List<long>> { new() },
                (acc, value) =>
                {
                    if (value.Length == 0)
                        acc.Add([]);
                    else
                        acc[^1].Add(long.Parse(new string(value)));
                    return acc;
                });

        var grandTotal = operations
            .Select((operation, exercise) =>
                CalculateExercise(operation, numbersGroupedByExercise[exercise]))
            .Sum();

        return grandTotal.ToString();
    }

    private static long CalculateExercise(char operation, IEnumerable<long> numbers) =>
         operation switch
         {
             '+' => numbers.Sum(),
             '*' => numbers.Aggregate(1L, (acc, val) => acc * val),
             _ => throw new InvalidOperationException($"Unknown operation '{operation}'."),
         };

}
