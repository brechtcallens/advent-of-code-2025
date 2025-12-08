namespace AdventOfCode2025.Solutions.Day01;

public class Day01Solver : BaseDaySolver
{
    public override int Day => 1;

    protected override string SolvePart1(string[] input, bool isExample)
    {
        var solution = 0;

        var position = 50;
        foreach (var line in input)
        {
            var rotation = line[0];
            var distance = int.Parse(line[1..]);

            position = (NextPosition(position, rotation, distance) + 100) % 100;

            if (position == 0)
                solution++;
        }

        return solution.ToString();
    }

    protected override string SolvePart2(string[] input, bool isExample)
    {
        var solution = 0;

        var position = 50;
        foreach (var line in input)
        {
            var rotation = line[0];
            var distance = int.Parse(line[1..]);

            var roundTrips = distance / 100;
            distance %= 100;

            var prevPosition = position;
            position = NextPosition(position, rotation, distance);

            solution += roundTrips;
            if ((position <= 0 || position >= 100) && prevPosition != 0)
                solution++;

            position = (position + 100) % 100;
        }

        return solution.ToString();
    }

    private static int NextPosition(int currentPosition, char rotation, int distance) =>
        rotation switch
        {
            'L' => currentPosition - distance,
            'R' => currentPosition + distance,
            _ => throw new InvalidOperationException($"Unknown rotation: {rotation}"),
        };
}
