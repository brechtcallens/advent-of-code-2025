using System.Data;

namespace AdventOfCode2025.Solutions.Day08;

public class Day08Solver : BaseDaySolver
{
    public override int Day => 8;

    protected override string SolvePart1(string[] input, bool isExample)
    {
        var boxes = GetBoxesFromInput(input);
        var boxDistances = GetConnectionDistancesFromBoxes(boxes);

        var workingSets = new Stack<HashSet<int>>(
            boxDistances[..boxes.Length]
                .Select(boxDistance => boxDistance.connection)
                .Select(connection => new HashSet<int>([connection.l, connection.r]))
        );

        int[] largestCircuits = [-1, -1, -1];
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
                var minIndex = largestCircuits[0] <= largestCircuits[1] && largestCircuits[0] <= largestCircuits[2]
                        ? 0
                        : largestCircuits[1] <= largestCircuits[2]
                            ? 1
                            : 2;
                largestCircuits[minIndex] = Math.Max(currentSet.Count, largestCircuits[minIndex]);
            }
        }
        return largestCircuits.Aggregate((acc, val) => acc * val).ToString();
    }

    protected override string SolvePart2(string[] input, bool isExample)
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
                            return (boxes[connection.l].x * boxes[connection.r].x).ToString();
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

    private static List<(double distance, (int l, int r) connection)> GetConnectionDistancesFromBoxes((long x, long y, long z)[] boxes)
    {
        var connectionDistances = new List<(double distance, (int l, int r) connection)>(boxes.Length * boxes.Length / 2);
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = i + 1; j < boxes.Length; j++)
            {
                var distance = Math.Sqrt(
                    Math.Pow(boxes[i].x - boxes[j].x, 2)
                    + Math.Pow(boxes[i].y - boxes[j].y, 2)
                    + Math.Pow(boxes[i].z - boxes[j].z, 2));
                connectionDistances.Add((distance, (i, j)));
            }
        }
        connectionDistances.Sort((a, b) => a.distance.CompareTo(b.distance));
        return connectionDistances;
    }

    private static (long x, long y, long z)[] GetBoxesFromInput(string[] input) =>
        [.. input.Select(line =>
            line.Split(",")
                .Select(long.Parse)
                .ToArray())
            .Select(line => (line[0], line[1], line[2]))];
}
