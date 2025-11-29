namespace AdventOfCode2025.Solutions;

public enum DayPart { Both = 0, Part1 = 1, Part2 = 2 };

public interface IDaySolver
{
    int Day { get; }
    void Solve(DayPart part, bool solveExample);
}