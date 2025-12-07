namespace AdventOfCode2025.Solutions.Day07;

public class Day07Solver : BaseDaySolver
{
    public override int Day => 7;

    protected override string SolvePart1(string[] grid)
    {
        var startPosition = (0, grid[0].IndexOf('S'));

        var visited = new HashSet<(int, int)>();
        var stack = new Stack<(int row, int column)>([startPosition]);
        while (stack.TryPop(out var position))
        {
            position = NextSplitPosition(grid, position);
            if (position.row < grid.Length && visited.Add(position))
            {
                stack.Push((position.row, position.column - 1));
                stack.Push((position.row, position.column + 1));
            }
        }
        return visited.Count.ToString();
    }

    protected override string SolvePart2(string[] grid)
    {
        var startPosition = (0, grid[0].IndexOf('S'));
        var solution = SolveBeamOptionsRecursively(grid, startPosition, []);
        return solution.ToString();
    }

    private static long SolveBeamOptionsRecursively(string[] grid, (int row, int column) startPosition, Dictionary<(int, int), long> cache)
    {
        var position = NextSplitPosition(grid, startPosition);
        if (position.row < grid.Length)
        {
            if (!cache.TryGetValue(position, out long subSolution))
            {
                subSolution = SolveBeamOptionsRecursively(grid, (position.row, position.column - 1), cache)
                    + SolveBeamOptionsRecursively(grid, (position.row, position.column + 1), cache);
                cache[position] = subSolution;
            }
            return subSolution;
        }
        return 1L;
    }

    private static (int row, int column) NextSplitPosition(string[] grid, (int row, int column) position)
    {
        while (position.row < grid.Length && grid[position.row][position.column] != '^')
            position.row++;
        return position;
    }
}
