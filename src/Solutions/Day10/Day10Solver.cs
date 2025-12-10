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

        var lights = (ushort)0;
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
        int total = 0;
        foreach (var line in input)
        {
            Console.WriteLine($"{++counter}");
            var (joltages, buttons) = ParseLineInputForPart2(line);

            var minCost = int.MaxValue;
            var bestCosts = new Dictionary<string, short>();
            var queue = new PriorityQueue<(short[] state, short cost), short>();
            queue.Enqueue((joltages, 0), Heuristic(joltages));
            while (queue.TryDequeue(out var current, out var currentEstimate))
            {
                var (state, cost) = current;
                // Console.WriteLine($"  {HashKey(state)} {cost} {queue.Count}");
                if (IsSolution(state))
                {
                    minCost = (int) cost;
                    break;
                }
                foreach (var button in buttons)
                {
                    var newState = state.ToArray();
                    var valid = true;
                    foreach (var position in button)
                    {
                        if (--newState[position] < 0)
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (valid)
                    {
                        var newStateKey = HashKey(newState);
                        var newStateCost = (short)(cost + 1);
                        if (!bestCosts.TryGetValue(newStateKey, out var bestCost) || bestCost > newStateCost)
                        {
                            bestCosts[newStateKey] = newStateCost;
                            queue.Enqueue((newState, newStateCost), (short)(newStateCost + Heuristic(newState)));
                        }
                    }
                }
            }
            total += minCost;
        }
        return total.ToString();
    }

    private static short Heuristic(short[] state) => state.Max();

    private static bool IsSolution(short[] state) => state.All(n => n == 0);

    private static string HashKey(short[] state) => string.Join(',', state);

    private (short[] joltages, List<short[]> buttons) ParseLineInputForPart2(string line)
    {
        var buttonsStart = line.IndexOf(" (");
        var joltagesStart = line.IndexOf(" {");

        var buttonParts = line[(buttonsStart + 1)..joltagesStart].Split(' ');
        var joltagePart = line[(joltagesStart + 1)..];

        var joltages = joltagePart[1..^1].Split(',').Select(short.Parse).ToArray();

        var buttons = new List<short[]>();
        foreach (var buttonPart in buttonParts)
        {
            var buttonArray = buttonPart[1..^1].Split(',').Select(short.Parse).ToArray();
            buttons.Add(buttonArray);
        }

        return (joltages, buttons);
    }
}
