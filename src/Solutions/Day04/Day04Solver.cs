using System.Diagnostics;

namespace AdventOfCode2025.Solutions.Day04;

public class Day04Solver : BaseDaySolver
{
    public override int Day => 4;

    protected override string SolvePart1(string[] input)
    {
        var solution = 0;
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                if (input[i][j] != '@')
                {
                    continue;
                }

                var rolls = 0;

                int d = 0;
                while (d < 9 && rolls < 4)
                {
                    var dx = (d / 3) - 1;
                    var dy = (d % 3) - 1;
                    if (!(dx == 0 && dy == 0) &&
                        0 <= i + dx && i + dx < input.Length &&
                        0 <= j + dy && j + dy < input[i].Length &&
                        input[i + dx][j + dy] == '@')
                    {
                        rolls++;
                    }
                    d++;
                }

                if (rolls < 4)
                {
                    solution++;
                }
            }
        }
        return solution.ToString();
    }

    protected override string SolvePart2(string[] input)
    {
        var inputChars = input.Select(line => line.ToCharArray()).ToArray();

        var totalRemovedRolls = 0;
        var removedRolls = 0;
        do
        {
            removedRolls = RemoveRolls(inputChars);
            totalRemovedRolls += removedRolls;
        }
        while (removedRolls > 0);

        return totalRemovedRolls.ToString();
    }

    private static int RemoveRolls(char[][] inputChars)
    {
        var rollsToBeRemoved = new List<(int, int)>();
        for (int i = 0; i < inputChars.Length; i++)
        {
            for (int j = 0; j < inputChars[i].Length; j++)
            {
                if (inputChars[i][j] != '@')
                {
                    continue;
                }

                var rolls = 0;

                int d = 0;
                while (d < 9 && rolls < 4)
                {
                    var dx = (d / 3) - 1;
                    var dy = (d % 3) - 1;
                    if (!(dx == 0 && dy == 0) &&
                        0 <= i + dx && i + dx < inputChars.Length &&
                        0 <= j + dy && j + dy < inputChars[i].Length &&
                        inputChars[i + dx][j + dy] == '@')
                    {
                        rolls++;
                    }
                    d++;
                }

                if (rolls < 4)
                {
                    rollsToBeRemoved.Add((i, j));
                }
            }
        }

        foreach (var rollToBeRemoved in rollsToBeRemoved)
        {
            inputChars[rollToBeRemoved.Item1][rollToBeRemoved.Item2] = '.';
        }

        return rollsToBeRemoved.Count;
    }
}
