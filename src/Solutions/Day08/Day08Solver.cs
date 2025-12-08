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
        var boxes = input.Select(line =>
            line.Split(",")
                .Select(long.Parse)
                .ToArray())
            .ToArray();

        var boxDistances = new List<(double distance, (int l, int r) position)>();
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = i + 1; j < boxes.Length; j++)
            {
                var distance = Math.Sqrt(
                    Math.Pow(boxes[i][0] - boxes[j][0], 2)
                    + Math.Pow(boxes[i][1] - boxes[j][1], 2)
                    + Math.Pow(boxes[i][2] - boxes[j][2], 2));
                boxDistances.Add((distance, (i, j)));
            }
        }
        boxDistances.Sort((a, b) => a.distance.CompareTo(b.distance));

        var workingSets = new Stack<HashSet<int>>();
        foreach (var (_, position) in boxDistances[..1000])
        {
            workingSets.Push([position.l, position.r]);
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

        return (finalSetSizes[^1] * finalSetSizes[^2] * finalSetSizes[^3]).ToString();
    }

    protected override string SolvePart2(string[] input)
    {
        var boxes = input.Select(line =>
            line.Split(",")
                .Select(long.Parse)
                .ToArray())
            .ToArray();

        var boxDistances = new List<(double distance, (int l, int r) position)>();
        for (int i = 0; i < boxes.Length; i++)
        {
            for (int j = i + 1; j < boxes.Length; j++)
            {
                var distance = Math.Sqrt(
                    Math.Pow(boxes[i][0] - boxes[j][0], 2)
                    + Math.Pow(boxes[i][1] - boxes[j][1], 2)
                    + Math.Pow(boxes[i][2] - boxes[j][2], 2));
                boxDistances.Add((distance, (i, j)));
            }
        }
        boxDistances.Sort((a, b) => a.distance.CompareTo(b.distance));

        var workingSets = new Stack<HashSet<int>>();
        var finalSets = new Stack<HashSet<int>>();
        foreach (var (_, position) in boxDistances)
        {
            workingSets = finalSets;
            workingSets.Push([position.l, position.r]);
            finalSets = [];

            var firstRun = true;
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
                    finalSets.Push(currentSet);
                    if (firstRun)
                    {
                        workingSets.Push(currentSet);
                        finalSets = workingSets;
                        workingSets = [];
                        break;
                    }
                }
                firstRun = false;
            }

            if (finalSets.Count == 1 && finalSets.Peek().Count == boxes.Length)
            {
                return (boxes[position.l][0] * boxes[position.r][0]).ToString();
            }
        }

        return "invalid";
    }
}
