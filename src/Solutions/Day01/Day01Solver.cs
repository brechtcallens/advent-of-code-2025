namespace AdventOfCode2025.Solutions.Day01;

public class Day01Solver : BaseDaySolver
{
    public override int Day => 1;

    protected override string SolvePart1(string[] input)
    {
        var solution = 0;

        var position = 50;
        foreach (var line in input)
        {
            var direction = line[0];
            var change = int.Parse(line[1..]);

            switch (direction)
            {
                case 'L':
                    position -= change;
                    break;
                case 'R':
                    position += change;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown direction: {direction}");
            }

            position = (position + 100) % 100;

            if (position == 0)
            {
                solution++;
            }
        }
        return $"{solution}";
    }

    protected override string SolvePart2(string[] input)
    {
        var solution = 0;

        var position = 50;
        foreach (var line in input)
        {
            var prevPosition = position;
            var direction = line[0];
            var change = int.Parse(line[1..]);
            
            var times = change / 100;
            change %= 100;

            switch (direction)
            {
                case 'L':
                    position -= change;
                    break;
                case 'R':
                    position += change;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown direction: {direction}");
            }

            solution += times;
            if (position <= 0 || position >= 100)
            {
                position = (position + 100) % 100;
                if (prevPosition != 0)
                {
                    solution ++;
                }
            }
        }
        
        return $"{solution}";
    }
}
