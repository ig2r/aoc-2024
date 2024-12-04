open System
open System.IO
open System.Text.RegularExpressions

let input = File.ReadAllText("input.txt")

let re = new Regex("\d+")

let partitionList (xs: 'a list) =
  let folder (state: 'a list * 'a list * int) elem =
    match state with
    | (ls, rs, i) when i % 2 = 0 -> (elem::ls, rs, i + 1)
    | (ls, rs, i) -> (ls, elem::rs, i + 1)
  let ls, rs, _ = List.fold folder ([], [], 0) xs
  ls, rs

// Apply regex to extract all numbers, parse, then split alternatingly into two evenly-sized lists (ls, rs).
let ls, rs =
  re.Matches(input)
  |> Seq.toList
  |> List.map (fun m -> Int32.Parse m.Value)
  |> partitionList

// Compute absolute pairwise distances between elements in sorted lists, then sum these distances to obtain total distance.
let deltas = List.map2 (fun (l: int) (r: int) -> Math.Abs(l - r)) (ls |> List.sort) (rs |> List.sort)
let totalDist = List.fold (fun state delta -> state + delta) 0 deltas
printfn "Total distance: %d" totalDist
