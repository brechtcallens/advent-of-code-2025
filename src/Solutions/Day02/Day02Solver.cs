using System.Text;

namespace AdventOfCode2025.Solutions.Day02;

public class Day02Solver : BaseDaySolver
{
    public override int Day => 2;

    protected override string SolvePart1(string[] input)
    {
        var ranges = input[0]
            .Split(',')
            .Select(range => range.Split("-"))
            .Select(splitRange => (left: long.Parse(splitRange[0]), right: long.Parse(splitRange[1])));

        var total = 0L;
        foreach (var (left, right) in ranges)
        {
            total += SolveRangeForChunkSize((left, right), 2).Sum();
        }
        return total.ToString();
    }

    protected override string SolvePart2(string[] input)
    {
        var ranges = input[0]
            .Split(',')
            .Select(range => range.Split("-"))
            .Select(splitRange => (left: long.Parse(splitRange[0]), right: long.Parse(splitRange[1])));

        var numbersToAdd = new HashSet<long>();
        foreach (var (left, right) in ranges)
        {
            for (int numberOfChunks = 2; numberOfChunks <= NumberOfDigits(right); numberOfChunks++)
            {
                var solutionForChunkSize = SolveRangeForChunkSize((left, right), numberOfChunks);
                numbersToAdd.UnionWith(solutionForChunkSize);
            }
        }
        return numbersToAdd.Sum().ToString();
    }

    private static int NumberOfDigits(long n) => (int)Math.Floor(Math.Log10(n)) + 1;

    private static List<long> SolveRangeForChunkSize((long, long) range, int numberOfChunks)
    {
        var (left, right) = range;
        var leftDigits = NumberOfDigits(left);
        var rightDigits = NumberOfDigits(right);
        var chunkSize = NumberOfDigits(right) / numberOfChunks;

        if (leftDigits != numberOfChunks * chunkSize && rightDigits != numberOfChunks * chunkSize)
        {
            return [];
        }

        long startChunk;
        if (leftDigits == numberOfChunks * chunkSize)
        {
            var firstChunk = left.ToString()[..chunkSize];
            var firstChunkNumber = long.Parse(firstChunk);
            var firstChunkRepeated = new StringBuilder().Insert(0, firstChunk, numberOfChunks).ToString();
            var firstChunkRepeatedNumber = long.Parse(firstChunkRepeated);
            startChunk = firstChunkRepeatedNumber >= left ? firstChunkNumber : firstChunkNumber + 1;
        }
        else
        {
            startChunk = (long)Math.Pow(10, chunkSize - 1);
        }

        long endChunk;
        if (rightDigits == numberOfChunks * chunkSize)
        {
            var firstChunk = right.ToString()[..chunkSize];
            var firstChunkNumber = long.Parse(firstChunk);
            var firstChunkRepeated = new StringBuilder().Insert(0, firstChunk, numberOfChunks).ToString();
            var firstChunkRepeatedNumber = long.Parse(firstChunkRepeated);
            endChunk = right >= firstChunkRepeatedNumber ? firstChunkNumber : firstChunkNumber - 1;
        }
        else
        {
            endChunk = (long)Math.Pow(10, chunkSize) - 1;
        }

        var invalidIds = new List<long>();
        for (long i = startChunk; i <= endChunk; i++)
        {
            invalidIds.Add(long.Parse(new StringBuilder().Insert(0, $"{i}", numberOfChunks).ToString()));
        }
        return invalidIds;
    }
}
