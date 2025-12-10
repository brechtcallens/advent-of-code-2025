using System.Collections;
using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode2025.Solutions.Day10;

public class Day10Solver : BaseDaySolver
{
    public override int Day => 10;

    protected override string SolvePart1(string[] input, bool isExample)
    {
        var total = 0;
        foreach (var line in input)
        {
            var (lights, buttons) = ParseLineInputForPart1(line);

            var currentDepth = 0;
            var stack = new Queue<(ushort state, ushort depth, ushort minButtonIndex)>([(lights, 0, 0)]);
            while (stack.TryDequeue(out var result))
            {
                var (state, depth, minButtonIndex) = result;
                for (ushort buttonIndex = minButtonIndex; buttonIndex < buttons.Count; buttonIndex++)
                {
                    var button = buttons[buttonIndex];
                    var stateWithButtonPressed = (ushort)(state ^ button);
                    var stateWithButtonPressedDepth = (ushort)(depth + 1);
                    
                    // If the button press caused a complete 0 state, we know this depth caused a solution. 
                    // We can save this minimum depth and clear the stack to cancel while loop.
                    if (stateWithButtonPressed == 0)
                    {
                        currentDepth = stateWithButtonPressedDepth;
                        stack.Clear();
                        break;
                    }
                    
                    // Continue BFS queue adding to check next ones.
                    stack.Enqueue((stateWithButtonPressed, stateWithButtonPressedDepth, buttonIndex));
                }
            }
            total += currentDepth;
        }
        return total.ToString();
    }

    private (ushort lights, List<ushort> buttons) ParseLineInputForPart1(string line)
    {
        var buttonsStart = line.IndexOf(" (");
        var joltagesStart = line.IndexOf(" {");

        var lightPart = line[..buttonsStart];
        var buttonParts = line[(buttonsStart + 1)..joltagesStart].Split(' ');
        var joltagePart = line[(joltagesStart + 1)..];

        var lights = (ushort) 0;
        var lightsBools = lightPart[1..^1].Select(c => c == '#').ToArray();
        for (ushort i = 0; i < lightsBools.Length; i++)
        {
            if (lightsBools[i])
            {
                lights |= (ushort)(1 << i);
            }
        }

        var buttons = new List<ushort>();
        foreach (var buttonPart in buttonParts)
        {
            var buttonBitArray = (ushort)0;
            var affectedLights = buttonPart[1..^1].Split(',').Select(ushort.Parse);
            foreach (var affectedLight in affectedLights)
            {
                buttonBitArray |= (ushort)(1 << affectedLight);
            }
            buttons.Add(buttonBitArray);
        }

        return (lights, buttons);
    }

    protected override string SolvePart2(string[] input, bool isExample)
    {
        var counter = 0;
        var total = 0;
        foreach (var line in input)
        {
            Console.WriteLine($"{++counter}");
            var parsedInput = new ParsedLineInputPart2(line);
            var joltages = parsedInput.Joltages;
            var buttons = parsedInput.Buttons;

            // Console.WriteLine($"{string.Join(", ", joltages)}");
            // foreach (var button in buttons)
            //     Console.WriteLine($"| {string.Join(", ", button)}");

            var currentDepth = 0;
            var firstItem = joltages.Select(_ => 0).ToArray();
            var stack = new Queue<(int[] bits, int depth)>([(firstItem, 0)]);
            while (stack.TryDequeue(out var result))
            {
                var (current, depth) = result;
                if (CompareCurrentToJoltages(current, joltages) != CompareResult.TooMuch)
                {
                    foreach (var button in buttons)
                    {
                        var newCurrent = current.ToArray();
                        foreach (var buttonPart in button)
                            newCurrent[buttonPart]++;
                        var newCurrentDepth = depth + 1;
                        if (CompareCurrentToJoltages(newCurrent, joltages) == CompareResult.Correct)
                        {
                            // If completely 0 we know we have a solution. Clear to cancel while loop.
                            currentDepth = newCurrentDepth;
                            stack.Clear();
                            break;
                        }
                        // Continue BFS queue adding to check next ones.
                        stack.Enqueue((newCurrent, newCurrentDepth));
                    }
                }
            }
            total += currentDepth;
        }
        return total.ToString();
    }

    private CompareResult CompareCurrentToJoltages(int[] current, int[] joltages)
    {
        var result = CompareResult.Correct;
        for (int i = 0; i < current.Length; i++)
        {
            if (current[i] > joltages[i])
            {
                return CompareResult.TooMuch;
            }
            else if (current[i] < joltages[i])
            {
                result = CompareResult.TooLittle;
            }
        }
        return result;
    }
}

internal readonly struct ParsedLineInputPart2
{
    public int[] Joltages { get; init; }
    public List<int[]> Buttons { get; init; }

    public ParsedLineInputPart2(string line)
    {
        var buttonsStart = line.IndexOf(" (");
        var joltagesStart = line.IndexOf(" {");

        var buttonParts = line[(buttonsStart + 1)..joltagesStart].Split(' ');
        var joltagePart = line[(joltagesStart + 1)..];

        Joltages = joltagePart[1..^1].Split(',').Select(int.Parse).ToArray();

        Buttons = [];
        foreach (var buttonPart in buttonParts)
        {
            var buttonArray = buttonPart[1..^1].Split(',').Select(int.Parse).ToArray();
            Buttons.Add(buttonArray);
        }
    }
}

internal enum CompareResult { TooMuch, TooLittle, Correct };