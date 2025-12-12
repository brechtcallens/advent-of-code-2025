using System.Collections;
using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;
using Google.OrTools.LinearSolver;

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
        double total = 0;
        foreach (var line in input)
        {
            var (joltages, buttons) = ParseLineInputForPart2(line);

            Solver solver = Solver.CreateSolver("SCIP");

            // Model each button as a variable in a LP equation problem.
            var buttonVariables = buttons
                .Select((_, index) => solver.MakeIntVar(0.0, double.PositiveInfinity, $"b{index}"))
                .ToArray();

            for (short joltage = 0; joltage < joltages.Length; joltage++)
            {
                // Find all buttons that affect the current joltage.
                var buttonsThatAffectJoltage = new List<short>();
                for (short b = 0; b < buttons.Count; b++)
                {
                    if (buttons[b].Contains(joltage))
                    {
                        buttonsThatAffectJoltage.Add(b);
                    }
                }

                // Create and add a linear expression of the affecting buttons with the expected joltage (e.g. b0 + b3 + b4 = 73)
                LinearExpr expression = buttonVariables[buttonsThatAffectJoltage[0]];
                for (int i = 1; i < buttonsThatAffectJoltage.Count; i++)
                {
                    expression += buttonVariables[buttonsThatAffectJoltage[i]];
                }
                solver.Add(expression == joltages[joltage]);
            }

            // Create the target for the LF problem, which is the minimal sum of all buttons.
            LinearExpr minExpression = buttonVariables[0];
            foreach (var variable in buttonVariables[1..])
            {
                minExpression += variable;
            }
            solver.Minimize(minExpression);

            // Solve the equations and add if there's an optimal solution.
            var resultStatus = solver.Solve();
            if (resultStatus != Solver.ResultStatus.OPTIMAL)
            {
                throw new Exception("Not supposed to not find a solution!");
            }
            total += solver.Objective().Value();
        }
        return total.ToString();
    }

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
