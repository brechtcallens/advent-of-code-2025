#!/bin/bash

if [[ $# -ne 1 ]]; then
   echo "Usage: $0 <day_nr>" 
   exit 1;
fi

day=$1
day_padded=$(printf "%02d" "$day")
day_dir="src/Solutions/Day${day_padded}"

# Create day directory if it does not exist yet
if [ -d "$day_dir" ]; then
   echo "Directory '$day_dir' already exists, skipping..."
else
   echo "Creating directory '$day_dir'"
   mkdir "$day_dir"
fi

# Create solution C# file if it does not exist yet
solution_file="${day_dir}/Day${day_padded}Solver.cs"
if [ -f "$solution_file" ]; then
   echo "Solution file '$solution_file' already exists, skipping..."
else
   echo "Creating solution file '$solution_file'"
   cat > "$solution_file" << EOF
namespace AdventOfCode2025.Solutions.Day${day_padded};

public class Day${day_padded}Solver : BaseDaySolver
{
    public override int Day => ${day};

    protected override string SolvePart1(string[] input, bool isExample)
    {
        throw new NotImplementedException("Missing implementation for part 1.");
    }

    protected override string SolvePart2(string[] input, bool isExample)
    {
        throw new NotImplementedException("Missing implementation for part 2.");
    }
}
EOF
fi

# Create example file if it does not exist yet
example_file="${day_dir}/example.txt"
if [ -f "$example_file" ]; then
   echo "Example file '$example_file' already exists, skipping..."
else
   echo "Creating empty example file '$example_file'"
   touch "$example_file"
fi

# Create input.txt file with input data from server if it does not exist yet
input_file="$day_dir/input.txt"
if [ -f "$input_file" ]; then
   echo "Input file '$input_file' already exists, skipping..."
else
   cookie_file=".session_cookie"
   if [ -f "$cookie_file" ]; then
      echo "Creating input file with data from server"
      session_cookie=$(cat $cookie_file)
      curl -s -H "cookie: $session_cookie" https://adventofcode.com/2025/day/$1/input | head -c -1 > $input_file
      lines=$(($(cat $input_file | wc -l) + 1))
      echo "Written $lines lines to '$input_file'"
   else
      echo "Cookie file '$cookie_file' does not exist, can't fetch input!"
      echo "Creating empty input file '$input_file'"
      touch "$input_file"
   fi
fi