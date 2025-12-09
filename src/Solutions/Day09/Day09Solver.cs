using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode2025.Solutions.Day09;

public class Day09Solver : BaseDaySolver
{
    public override int Day => 9;

    protected override string SolvePart1(string[] input, bool isExample)
    {
        var points = input
            .Select(line => line.Split(',').Select(long.Parse).ToArray())
            .Select(line => (X: line[0], Y: line[1]))
            .ToArray();

        var maxSurface = -1L;
        for (int i = 0; i < points.Length; i++)
        {
            for (int j = i + 1; j < points.Length; j++)
            {
                maxSurface = Math.Max(
                    (Math.Abs(points[i].X - points[j].X) + 1) * (Math.Abs(points[i].Y - points[j].Y) + 1),
                    maxSurface
                );
            }
        }
        return maxSurface.ToString();
    }

    protected override string SolvePart2(string[] input, bool isExample)
    {
        var points = input
            .Select(line => line.Split(',').Select(long.Parse).ToArray())
            .Select(line => (X: line[0], Y: line[1]))
            .ToArray(); 

        var pointsInGraph = new HashSet<(long X, long Y)>();
        var verticalLines = new List<((long X, long Y) minPoint, (long X, long Y) maxPoint)>();
        var horizontalLines = new List<((long X, long Y) minPoint, (long X, long Y) maxPoint)>();
        for (int p = 0; p < points.Length; p++)
        {
            var p1 = points[p];
            var p2 = points[(p + 1) % points.Length];
            if (p1.X == p2.X)
            {
                var (minPoint, maxPoint) = p1.Y < p2.Y ? (p1, p2) : (p2, p1);
                verticalLines.Add((minPoint, maxPoint));
            }
            else
            {
                var (minPoint, maxPoint) = p1.X < p2.X ? (p1, p2) : (p2, p1);
                horizontalLines.Add((minPoint, maxPoint));
            }
            pointsInGraph.Add(p1);
        }
        verticalLines.Sort((line1, line2) => line1.minPoint.X.CompareTo(line2.minPoint.X));

        var maxSurface = -1L;
        for (int i = 0; i < points.Length; i++)
        {
            Console.WriteLine($"{i}/{points.Length}");
            for (int j = i + 1; j < points.Length; j++)
            {
                var p1 = points[i];
                var p2 = points[j];

                // If wall, skip for now...
                if (p1.X == p2.X || p1.Y == p2.Y)
                {
                    continue;
                }

                // If not nice new big surface, why even bother.
                var surface = (Math.Abs(p1.X - p2.X) + 1) * (Math.Abs(p1.Y - p2.Y) + 1);
                if (surface <= maxSurface)
                {
                    continue;
                }

                var allStillOkay = true;
                var minX = Math.Min(p1.X, p2.X);
                var minY = Math.Min(p1.Y, p2.Y);
                var maxX = Math.Max(p1.X, p2.X);
                var maxY = Math.Max(p1.Y, p2.Y);

                for (int dx = 0; dx <= maxX - minX; dx++)
                {
                    if (!IsInGraph(pointsInGraph, verticalLines, horizontalLines, (minX + dx, minY)))
                    {
                        allStillOkay = false;
                        break;
                    }
                    if (!IsInGraph(pointsInGraph, verticalLines, horizontalLines, (minX + dx, maxY)))
                    {
                        allStillOkay = false;
                        break;
                    }
                }
                if (!allStillOkay)
                {
                    continue;
                }

                for (int dy = 0; dy <= maxY - minY; dy++)
                {
                    
                    if (!IsInGraph(pointsInGraph, verticalLines, horizontalLines, (minX, minY + dy)))
                    {
                        allStillOkay = false;
                        break;
                    }
                    if (!IsInGraph(pointsInGraph, verticalLines, horizontalLines, (maxX, minY + dy)))
                    {
                        allStillOkay = false;
                        break;
                    }
                }

                if (allStillOkay)
                {
                    maxSurface = surface;
                    Console.WriteLine($"New max surface: {maxSurface} (points {p1}, {p2})");
                }
            }
        }
        return maxSurface.ToString();
    }

    private bool IsInGraph(
        HashSet<(long X, long Y)> pointsInGraph,
        List<((long X, long Y) minPoint, (long X, long Y) maxPoint)> verticalLines,
        List<((long X, long Y) minPoint, (long X, long Y) maxPoint)> horizontalLines,
        (long X, long Y) point)
    {
        if (pointsInGraph.Contains(point))
        {
            return true;
        }

        bool isInGraph = false;
        foreach (var verticalLine in verticalLines)
        {
            if (verticalLine.minPoint.X > point.X)
            {
                break;
            }

            if (verticalLine.minPoint.Y <= point.Y && point.Y < verticalLine.maxPoint.Y)
            {
                isInGraph = !isInGraph;
            }
        }

        if (!isInGraph)
        {
            foreach (var horizontalLine in horizontalLines)
            {
                if (horizontalLine.minPoint.Y == point.Y &&
                    horizontalLine.minPoint.X <= point.X &&
                    point.X <= horizontalLine.maxPoint.X)
                {
                    isInGraph = true;
                    // Console.WriteLine("YAY");
                    break;
                }
            }
        }
        else if (isInGraph)
        {
            pointsInGraph.Add(point);
        }

        // Console.WriteLine($"{point} is in graph: {isInGraph}");
        return isInGraph;
    }
}
