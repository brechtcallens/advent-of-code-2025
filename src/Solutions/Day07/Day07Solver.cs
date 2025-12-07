namespace AdventOfCode2025.Solutions.Day07;

public class Day07Solver : BaseDaySolver
{
    public override int Day => 7;

    protected override string SolvePart1(string[] input)
    {
        var grid = input.Select(line => line.ToCharArray()).ToArray();
        var startPosition = (0, Array.FindIndex(grid[0], c => c == 'S'));

        var visited = new HashSet<(int, int)>();        
        var stack = new Stack<(int row, int column)>([startPosition]);
        while (stack.TryPop(out var position))
        {
            while (position.row < grid.Length && grid[position.row][position.column] != '^')
            {
                position.row++;
            }

            if (position.row < grid.Length && !visited.Contains(position))
            {
                visited.Add(position);
                stack.Push((position.row, position.column - 1));
                stack.Push((position.row, position.column + 1));
            }
        }

        return visited.Count.ToString();
    }

    protected override string SolvePart2(string[] input)
    {
        var grid = input.Select(line => line.ToCharArray()).ToArray();
        var startPosition = (0, Array.FindIndex(grid[0], c => c == 'S'));
        var cache = new Dictionary<(int, int), long>();

        var solution = SolveRecursively(grid, startPosition, cache);

        return solution.ToString();
    }

    private static long SolveRecursively(char[][] grid, (int row, int column) startPosition, Dictionary<(int, int), long> cache)
    {
        var position = startPosition;
        while (position.row < grid.Length && grid[position.row][position.column] != '^')
        {
            position.row++;
        }

        if (position.row < grid.Length)
        {
            if (!cache.TryGetValue(position, out long subSolution))
            {
                subSolution = SolveRecursively(grid, (position.row, position.column - 1), cache)
                    + SolveRecursively(grid, (position.row, position.column + 1), cache);
                cache[position] = subSolution;
            }
            return subSolution;
        }
        return 1L;
    }
}
