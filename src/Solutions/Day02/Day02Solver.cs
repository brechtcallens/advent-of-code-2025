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
        var ranges = input[0]
            .Split(',')
            .Select(range => range.Split("-"))
            .Select(splitRange => (long.Parse(splitRange[0]), long.Parse(splitRange[1])));
        
        var conditionRegex = new Regex(regexString);       

        long invalidIds = 0;
        foreach (var (start, end) in ranges)
        {
            for (long n = start; n <= end; n++)
            {
                if (conditionRegex.IsMatch(n.ToString()))
                {
                    invalidIds += n;
                }
            }
        }        
        return invalidIds.ToString();
    }
}
