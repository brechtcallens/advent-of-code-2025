using System.Text.RegularExpressions;

namespace AdventOfCode2025.Solutions.Day02;

public class Day02Solver : BaseDaySolver
{
    public override int Day => 2;

    protected override string SolvePart1(string[] input) =>
         SolveGeneric(input, @"^(\d+)\1$");

    protected override string SolvePart2(string[] input) =>
        SolveGeneric(input, @"^(\d+)\1+$");

    private static string SolveGeneric(string[] input, string regexString)
    {
        long invalidIds = 0;
        var ranges = input[0].Split(',')
            .ToList()
            .Select(range => range.Split("-").ToList());

        foreach (var range in ranges)
        {
            var start = range[0];
            var end = range[1];

            var startNumber = long.Parse(start);
            var endNumber = long.Parse(end);

            for (long n = startNumber; n <= endNumber; n++)
            {
                var conditionRegex = new Regex(regexString);
                var check = conditionRegex.Match(n.ToString());
                if (check.Success)
                {
                    invalidIds += n;
                }
            }
        }
        return invalidIds.ToString();
    }
}
