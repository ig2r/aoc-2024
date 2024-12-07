open System
open System.IO

let input = File.ReadAllLines("input.txt")

// Represents a single line in the input.
type Equation = {
  Result: int64;
  Inputs: list<int64>;
}

let parseLine (line: string) =
  let parts = line.Split(':')
  { Result = Int64.Parse parts[0];
    Inputs =
      parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
      |> Array.map Int64.Parse
      |> Array.toList }

let canEquationBeValid (equation: Equation) =
  let rec check intermediateResult remainingInput =
    match remainingInput with
    | [] -> intermediateResult = equation.Result
    | x :: xs ->
      if intermediateResult > equation.Result
        then false
        else check (intermediateResult + x) xs || check (intermediateResult * x) xs
  check 0 equation.Inputs

let equations = Array.map parseLine input

// Solution for part 1:
let validEquations = Array.filter canEquationBeValid equations
let totalCalibrationResult = Array.fold (fun sum e -> sum + e.Result) 0L validEquations
printfn "totalCalibrationResult: %d" totalCalibrationResult
