namespace AdventOfCode2025.Solutions.Day12;

public class Day12Solver : BaseDaySolver
{
    public override int Day => 12;

    protected override string SolvePart1(string[] input, bool isExample)
    {
        var boxesParts = new List<List<string>>();
        var lastPart = new List<String>();
        foreach (var line in input)
        {
            if (line.Trim() == "")
            {
                boxesParts.Add(lastPart);
                lastPart = [];
            }
            else
            {
                lastPart.Add(line);
            }
        }

        var boxes = new List<int>();
        foreach (var boxPart in boxesParts)
        {
            var hashtags = 0;
            foreach (var boxLine in boxPart[1..])
            {
                hashtags += boxLine.Where(c => c == '#').Count();
            }
            boxes.Add(hashtags);
        }

        var total = 0;
        foreach (var gridLine in lastPart)
        {
            var splittedParts = gridLine.Split(": ");
            var grid = splittedParts[0].Split("x").Select(int.Parse).ToArray();
            var amounts = splittedParts[1].Split(" ").Select(int.Parse).ToArray();

            var gridSum = grid[0] * grid[1];
            var amountsSum = 0;
            for (int i = 0; i < amounts.Length; i++)
            {
                amountsSum += amounts[i] * boxes[i];
            }

            if (gridSum >= amountsSum)
            {
                total++;
            }
        }

        return total.ToString();
    }

    protected override string SolvePart2(string[] input, bool isExample)
    {
        return "There is no part 2!";
    }
}
