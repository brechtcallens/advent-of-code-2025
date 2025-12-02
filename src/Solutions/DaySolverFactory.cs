using AdventOfCode2025.Solutions.Day01;
using AdventOfCode2025.Solutions.Day02;

namespace AdventOfCode2025.Solutions;

public class DaySolverFactory
{
    private static readonly IEnumerable<IDaySolver> _solvers =
    [
        new Day01Solver(),
        new Day02Solver()
    ];

    private readonly Dictionary<int, IDaySolver> _solversDictionary = 
        _solvers.ToDictionary(solver => solver.Day);

    public IDaySolver GetSolver(int day)
    {
        _solversDictionary.TryGetValue(day, out var solver);
        if (solver == null)
        {
            throw new ArgumentException($"No solver has been configured for day {day}.", nameof(day));
        }
        return solver;
    }
}