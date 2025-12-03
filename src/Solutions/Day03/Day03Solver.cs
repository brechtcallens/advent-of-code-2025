namespace AdventOfCode2025.Solutions.Day03;

public class Day03Solver : BaseDaySolver
{
    public override int Day => 3;

    protected override string SolvePart1(string[] input)
    {
        var solution = 0;
        foreach (var line in input)
        {
            var numbers = line.Select(c => int.Parse(c.ToString())).ToList();
            var maxLeft = int.Parse(line[^2].ToString());
            var maxRight = int.Parse(line[^1].ToString());

            for (int i = line.Length - 3; i >= 0; i--)
            {
                var current = int.Parse(line[i].ToString());
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
            var numbers = line.Select(c => int.Parse(c.ToString())).ToList();
            var maxList = numbers.GetRange(numbers.Count - 12, 12);
            
            for (int i = numbers.Count - 13; i >= 0; i--)
            {
                var current = numbers[i];
                var currentMaxIndex = 0;
                while (currentMaxIndex < maxList.Count && current >= maxList[currentMaxIndex])
                {
                    var prevCurrent = current;
                    current = maxList[currentMaxIndex];
                    maxList[currentMaxIndex] = prevCurrent;
                    currentMaxIndex++;
                }
            }

            var largestJoltage = long.Parse(string.Join("", maxList));
            solution += largestJoltage;
        }
        return solution.ToString();
    }
}
