namespace AdventOfCode2025.Solutions.Day03;

public class Day03Solver : BaseDaySolver
{
    public override int Day => 3;

    protected override string SolvePart1(string[] input)
    {
        var solution = 0;
        foreach (var line in input)
        {
            var maxLeft = line[^2];
            var maxRight = line[^1];

            for (int i = line.Length - 3; i >= 0; i--)
            {
                var current = line[i];
                if (current >= maxLeft)
                {
                    if (maxLeft > maxRight)
                    {
                        maxRight = maxLeft;
                    }
                    maxLeft = current;
                }
            }

            var largestJoltage = int.Parse($"{maxLeft}{maxRight}");
            solution += largestJoltage;
        }
        return solution.ToString();
    }

    protected override string SolvePart2(string[] input)
    {
        long solution = 0;
        foreach (var line in input)
        {
            var maxList = line[(line.Length - 12)..line.Length].ToCharArray();

            for (int i = line.Length - maxList.Length - 1; i >= 0; i--)
            {
                var current = line[i];
                var currentMaxIndex = 0;
                while (currentMaxIndex < maxList.Length && current >= maxList[currentMaxIndex])
                {
                    (maxList[currentMaxIndex], current) = (current, maxList[currentMaxIndex]);
                    currentMaxIndex++;
                }
            }

            var largestJoltage = long.Parse(string.Join("", maxList));
            solution += largestJoltage;
        }
        return solution.ToString();
    }
}
