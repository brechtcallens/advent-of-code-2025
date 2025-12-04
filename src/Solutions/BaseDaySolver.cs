using System.Diagnostics;

namespace AdventOfCode2025.Solutions;

public abstract class BaseDaySolver : IDaySolver
{
    public abstract int Day { get; }

    protected abstract string SolvePart1(string[] input);

    protected abstract string SolvePart2(string[] input);

    public void Solve(DayPart part, bool solveExample)
    {
        DayPart[] partsToSolve = part == DayPart.Both
            ? [DayPart.Part1, DayPart.Part2]
            : [part];

        var input = GetInput(solveExample);

        var originalOut = Console.Out;
        foreach (var partToSolve in partsToSolve)
        {
            using (new CustomTimeLogger(originalOut, $"solve day {Day} [part {(partToSolve == DayPart.Part1 ? 1 : 2)}]{(solveExample ? " (example)" : "")}"))
            {
                var stopwatch = Stopwatch.StartNew();
                var solution = partToSolve == DayPart.Part1 ? SolvePart1(input) : SolvePart2(input);
                stopwatch.Stop();
                Console.WriteLine($"SOLUTION: {solution} (elapsed: {stopwatch.ElapsedMilliseconds}ms)");
            }
        }
    }

    private string GetInputFilePath(bool solveExample) =>
        $"Solutions/Day{Day:00}/{(solveExample ? "example" : "input")}.txt";

    private string[] GetInput(bool solveExample) =>
        File.ReadAllLines(GetInputFilePath(solveExample));
}