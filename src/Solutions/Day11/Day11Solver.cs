using System.Text.RegularExpressions;

namespace AdventOfCode2025.Solutions.Day11;

public class Day11Solver : BaseDaySolver
{
    public override int Day => 11;

    protected override string SolvePart1(string[] input, bool isExample)
    {
        var connections = new Dictionary<string, string[]>();
        foreach (var line in input)
        {
            var from = line[..3];
            var tos = line[5..].Split(' ');
            connections[from] = tos;
        }
        return FindPathsToOut("you", connections, [], []).ToString();
    }

    private static int FindPathsToOut(string from, Dictionary<string, string[]> connections, HashSet<string> visits, Dictionary<string, int> cache)
    {
        if (from == "out")
        {
            return 1;
        }

        if (cache.TryGetValue(from, out var result))
        {
            return result;
        }

        var total = 0;
        foreach (var next in connections[from])
        {
            if (visits.Add(next))
            {
                total += FindPathsToOut(next, connections, visits, cache);
                visits.Remove(next);
            }
        }
        cache[from] = total;
        return total;
    }

    protected override string SolvePart2(string[] input, bool isExample)
    {
        var connections = new Dictionary<string, string[]>();
        foreach (var line in input)
        {
            var from = line[..3];
            var tos = line[5..].Split(' ');
            connections[from] = tos;
        }
        return FindPathsToOutThatContainDacAndFft(("svr", false, false), connections, [], []).ToString();
    }

    private static long FindPathsToOutThatContainDacAndFft(
        (string from, bool visitedDac, bool visitedFft) fromObject,
        Dictionary<string, string[]> connections, 
        HashSet<string> visits, 
        Dictionary<(string, bool, bool), long> cache)
    {
        (string from, bool visitedDac, bool visitedFft) = fromObject;
        if (from == "out")
        {
            return visitedDac && visitedFft ? 1 : 0;
        }

        if (cache.TryGetValue(fromObject, out var result))
        {
            return result;
        }

        var total = 0L;
        foreach (var next in connections[from])
        {
            if (visits.Add(next))
            {
                var nextObject = (next, visitedDac || (next == "dac"), visitedFft || (next == "fft"));
                total += FindPathsToOutThatContainDacAndFft(nextObject, connections, visits, cache);
                visits.Remove(next);
            }
        }
        cache[fromObject] = total;
        return total;
    }
}
