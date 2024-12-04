open System
open System.IO
open System.Text.RegularExpressions

let input = File.ReadAllLines("input.txt")
let re = new Regex(@"(\d+)\s+(\d+)")

let parseLine line =
  let groups = re.Match(line).Groups
  let x, y = (Int32.Parse groups[1].Value), (Int32.Parse groups[2].Value)
  x, y

let ls, rs =
  input
  |> Array.map parseLine
  |> Array.fold (fun (xs, ys) (x, y) -> (x :: xs, y :: ys)) ([], [])

// Compute absolute pairwise distances between elements in sorted lists, then sum these distances to obtain total distance.
let ls_sorted, rs_sorted = (List.sort ls), (List.sort rs)
let deltas = List.map2 (fun (l: int) (r: int) -> Math.Abs(l - r)) ls_sorted rs_sorted
let totalDist = List.fold (fun sum delta -> sum + delta) 0 deltas
printfn "Total distance: %d" totalDist
