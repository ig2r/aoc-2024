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

let canEquationBeValid (operators: list<int64 -> int64 -> int64>) (equation: Equation) =
  let rec check intermediateResult remainingInput =
    match remainingInput with
    | [] -> intermediateResult = equation.Result
    | x :: xs ->
      if intermediateResult > equation.Result
        then false
        else List.exists (fun op -> check (op intermediateResult x) xs) operators
  check 0 equation.Inputs

let equations = Array.map parseLine input

// Solution to part 1, which permits only left-to-right addition and multiplication operators.
let solvePart1 =
  let operators = [ (+); (*) ]
  equations
    |> Array.filter (canEquationBeValid operators)
    |> Array.fold (fun sum e -> sum + e.Result) 0L

// Defines a concatenation operator '||' such that 15 || 6 = 156.
let concatDigits (x: int64) (y: int64) =
  let shiftFactor = int(Math.Log10(double(y))) + 1
  x * int64(Math.Pow(10, shiftFactor)) + y

// Solution to part 2, which includes the special '||' concatenation operator as well.
let solvePart2 =
  let operators = [ (+); (*); concatDigits ]
  equations
    |> Array.filter (canEquationBeValid operators)
    |> Array.fold (fun sum e -> sum + e.Result) 0L

printfn "Total calibration result (part 1): %d" solvePart1
printfn "Total calibration result (part 2): %d" solvePart2
