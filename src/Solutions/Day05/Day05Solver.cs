namespace AdventOfCode2025.Solutions.Day05;

public class Day05Solver : BaseDaySolver
{
    public override int Day => 5;

    protected override string SolvePart1(string[] input)
    {
        var ranges = new List<(long start, long end)>();
        var ingredients = new List<long>();
        var parsingIngredients = false;
        foreach (var line in input)
        {
            if (line.Length == 0)
            {
                parsingIngredients = true;
            }
            else if (parsingIngredients)
            {
                ingredients.Add(long.Parse(line));
            }
            else
            {
                var parts = line.Split('-');
                var start = long.Parse(parts[0]);
                var end = long.Parse(parts[1]);
                ranges.Add((start, end));
            }
        }

        var freshIngredientCount = 0;
        foreach (var ingredient in ingredients)
        {
            var isFresh = false;
            var rangeIndex = 0;
            while (rangeIndex < ranges.Count && !isFresh)
            {
                if (ranges[rangeIndex].start <= ingredient && ingredient <= ranges[rangeIndex].end)
                {
                    isFresh = true;
                }
                rangeIndex++;
            }
            if (isFresh)
            {
                freshIngredientCount++;
            }
        }

        return freshIngredientCount.ToString();
    }

    protected override string SolvePart2(string[] input)
    {
        var ranges = new List<(long start, long end)>();
        foreach (var line in input)
        {
            if (line.Length == 0)
            {
                // We don't care about what comes after...
                break; 
            }
            else
            {
                var parts = line.Split('-');
                var start = long.Parse(parts[0]);
                var end = long.Parse(parts[1]);
                ranges.Add((start, end));
            }
        }
        ranges.Sort();

        var currentStart = ranges[0].start;
        var currentEnd = ranges[1].end;
        var freshIngredientCount = currentEnd - currentStart + 1;
        foreach (var (start, end) in ranges[1..])
        {
            if (end <= currentEnd)
            {
                // New range is contained by the previous one, so no need to do anything.
            }
            else if (start > currentEnd)
            {
                // We had a range and now we're jumping forward to a new non-overlapping range.
                currentStart = start;
                currentEnd = end;
                freshIngredientCount += currentEnd - currentStart + 1;
            }
            else
            {
                // We have some overlap between the new range and the old range.
                // S1.....E1
                //    S2.......E2
                freshIngredientCount += end - currentEnd;
                currentStart = start;
                currentEnd = end;
            }
        }

        return freshIngredientCount.ToString();
    }
}
