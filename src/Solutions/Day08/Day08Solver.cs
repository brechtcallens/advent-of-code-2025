using System.Dynamic;
using System.Formats.Asn1;
using System.Linq.Expressions;
using System.Security.AccessControl;

namespace AdventOfCode2025.Solutions.Day08;

public class Day08Solver : BaseDaySolver
{
    public override int Day => 8;

    protected override string SolvePart1(string[] input)
    {
        var boxes = GetBoxesFromInput(input);
        var boxDistances = GetConnectionDistancesFromBoxes(boxes);

        var workingSets = new Stack<HashSet<int>>();
        foreach (var (_, connection) in boxDistances[..1000])
        {
            workingSets.Push([connection.l, connection.r]);
        }

        var finalSetSizes = new List<int>();
        while (workingSets.TryPop(out var currentSet))
        {
            bool mergedWithSomething = false;
            foreach (var workingSet in workingSets)
            {
                if (currentSet.Intersect(workingSet).Any())
                {
                    workingSet.UnionWith(currentSet);
                    mergedWithSomething = true;
                    break;
                }
            }
            if (!mergedWithSomething)
            {
                finalSetSizes.Add(currentSet.Count);
            }
        }
        finalSetSizes.Sort();

        return finalSetSizes[^3..]
            .Aggregate((acc, val) => acc * val)
            .ToString();
    }

    protected override string SolvePart2(string[] input)
    {
        var boxes = GetBoxesFromInput(input);
        var connectionDistances = GetConnectionDistancesFromBoxes(boxes);

        var finalizedSets = new Stack<HashSet<int>>();
        foreach (var (_, connection) in connectionDistances)
        {
            var workingSets = finalizedSets;
            workingSets.Push([connection.l, connection.r]);
            finalizedSets = [];

            var firstRun = true;
            while (workingSets.TryPop(out var currentWorkingSet))
            {
                bool mergedWithSomething = false;
                foreach (var otherWorkingSet in workingSets)
                {
                    if (currentWorkingSet.Intersect(otherWorkingSet).Any())
                    {
                        otherWorkingSet.UnionWith(currentWorkingSet);
                        mergedWithSomething = true;

                        // If the other working set now contains all boxes, we knew this connection's merge was the cause and we should
                        // stop and return the answer.
                        if (otherWorkingSet.Count == boxes.Length)
                        {
                            return (boxes[connection.l][0] * boxes[connection.r][0]).ToString();
                        }
                        break;
                    }
                }
                if (!mergedWithSomething)
                {
                    finalizedSets.Push(currentWorkingSet);

                    // Optimization: If nothing was merged in first run, we know the sets are still distinct.
                    if (firstRun)
                    {
                        workingSets.Push(currentWorkingSet);
                        finalizedSets = workingSets;
                        break;
                    }
                }
                firstRun = false;
            }
        }

        return "invalid";
    }

    private static List<(double distance, (int l, int r) connection)> GetConnectionDistancesFromBoxes(long[][] boxes)
    {
        var connectionDistances = new List<(double distance, (int l, int r) connection)>(boxes.Length * boxes.Length / 2);
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = i + 1; j < boxes.Length; j++)
            {
                var distance = Math.Sqrt(
                    Math.Pow(boxes[i][0] - boxes[j][0], 2)
                    + Math.Pow(boxes[i][1] - boxes[j][1], 2)
                    + Math.Pow(boxes[i][2] - boxes[j][2], 2));
                connectionDistances.Add((distance, (i, j)));
            }
        }
        connectionDistances.Sort((a, b) => a.distance.CompareTo(b.distance));
        return connectionDistances;
    }

    private static long[][] GetBoxesFromInput(string[] input) =>
        [.. input.Select(line =>
            line.Split(",")
                .Select(long.Parse)
                .ToArray())];
}
