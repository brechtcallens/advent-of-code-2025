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
        return FindPathsToOutThatContainDacAndFft(("svr", false, false, false, false), connections, [], []).ToString();
    }

    private static int FindPathsToOutThatContainDacAndFft(
        (string from, bool firstDac, bool firstFft, bool secondDac, bool secondFft) fromObject,
        Dictionary<string, string[]> connections, 
        HashSet<string> visits, 
        Dictionary<(string, bool, bool, bool, bool), int> cache)
    {
        (string from, bool firstDac, bool firstFft, bool secondDac, bool secondFft) = fromObject;
        if (from == "out")
        {
            return ((firstDac && secondFft) || (firstFft && secondDac)) ? 1 : 0;
        }

        if (cache.TryGetValue(fromObject, out var result))
        {
            return result;
        }

        var total = 0;
        foreach (var next in connections[from])
        {
            if (visits.Add(next))
            {
                var firstSet = firstDac || firstFft;
                var isDac = next == "dac";
                var isFft = next == "fft";
                var nextObject = firstSet
                    ? (next, firstDac, firstFft, secondDac || isDac, secondFft || isFft)
                    : (next, firstDac || isDac, firstFft || isFft, secondDac, secondFft);
                total += FindPathsToOutThatContainDacAndFft(nextObject, connections, visits, cache);
                visits.Remove(next);
            }
        }
        cache[fromObject] = total;
        return total;
    }
}
