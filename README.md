# Advent Of Code (2025)

.NET solutions for Advent of Code 2025.

## Setup

Steps to setup the project so one can solve/run these tasks. Make sure to run on WSL or any Linux distro.

1. Copy your advent of code session cookie from your browser console into a file called `.session-cookie` to autofetch the inputs.
2. Install dotnet 9.0 or higher.

## Code

Run the following code for each new day.

1. Run `./setup.sh <day_nr>` to create the solver template and fetch the input into directory `src/Solutions/Day<day_nr>`.
2. Manually copy or fill in some input in the generated `example.txt` file.
3. Add `Day<day_nr>Solver.cs` as a solver to the list of solvers in `src/Solutions/DaySolverFactory.cs`.
4. Open the `Day<day_nr>Solver.cs` and solve it!
5. Run `cd src && dotnet run -- solve <day_nr>` to solve the input file and `cd src && dotnet run -- solve <day_nr> --example` for the example input.
