using System.Text.RegularExpressions;

namespace AdventOfCode2025.Solutions.Day06;

public class Day06Solver : BaseDaySolver
{
    public override int Day => 6;

    protected override string SolvePart1(string[] input)
    {
        var numbersLines = input[..^1].Select(line =>
            Regex.Split(line, @"\s+")
                .Where(s => !string.IsNullOrEmpty(s))
                .Select(s => long.Parse(s.Trim()))
                .ToArray())
            .ToArray();
        
        var operationLine = Regex.Split(input[^1], @"\s+")
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(s => s.Trim())
            .ToArray();

        var grandTotal = 0L;
        for (int exercise = 0; exercise < operationLine.Length; exercise++)
        {
            var op = operationLine[exercise];
            switch (op)
            {
                case "+":
                    {
                        grandTotal += numbersLines
                            .Select(line => line[exercise])
                            .Sum();
                        break;
                    }
                case "*":
                    {
                        grandTotal += numbersLines
                            .Select(line => line[exercise])
                            .Aggregate(1L, (acc, val) => acc * val);
                        break;
                    }
                default:
                    throw new InvalidOperationException($"Unknown operation '{op}' for exercise {exercise}.");
            }
        }
        return grandTotal.ToString();
    }

    protected override string SolvePart2(string[] input)
    {
        var operationLine = Regex.Split(input[^1], @"\s+")
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(s => s.Trim())
            .ToArray();
        
        var grid = input[..^1]
            .Select(line => line.ToCharArray())
            .ToArray();
        
        var numbersLists = new List<List<long>>();
        var numbers = new List<long>();
        for (int j = 0; j < grid[0].Length; j++)
        {
            var x = new string(grid
                .Select(line => line[j])
                .Where(character => character != ' ')
                .ToArray());
            
            if (x.Length != 0)
            {
                numbers.Add(long.Parse(x));
            }
            else
            {
                numbersLists.Add(numbers);
                numbers = new List<long>();
            }
        }
        numbersLists.Add(numbers);

        var grandTotal = 0L;
        for (int exercise = 0; exercise < operationLine.Length; exercise++)
        {
            var op = operationLine[exercise];
            switch (op)
            {
                case "+":
                    {
                        grandTotal += numbersLists[exercise].Sum();
                        break;
                    }
                case "*":
                    {
                        grandTotal += numbersLists[exercise].Aggregate(1L, (acc, val) => acc * val);
                        break;
                    }
                default:
                    throw new InvalidOperationException($"Unknown operation '{op}' for exercise {exercise}.");
            }
        }
        return grandTotal.ToString();
    }
}
