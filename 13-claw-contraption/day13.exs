Mix.install([{:nx, "~> 0.9"}])

{:ok, data} = File.read("input.txt")

claw_machines =
  data
  |> String.split("\n")
  |> Enum.chunk_every(4)

parse_coords = fn line ->
  Regex.run(~r/X.(\d+), Y.(\d+)/, line, capture: :all_but_first)
  |> Enum.map(&String.to_integer/1)
end

epsilon = 0.00001

price = fn lines ->
  [button_a_line, button_b_line, prize_line | _] = lines

  # Solve linear equation AX = B for X.
  button_a_xy = parse_coords.(button_a_line)
  button_b_xy = parse_coords.(button_b_line)
  a = Nx.tensor([button_a_xy, button_b_xy]) |> Nx.transpose()
  b = Nx.tensor(parse_coords.(prize_line))
  x = Nx.LinAlg.solve(a, b)

  # Sum error squares to gauge if there's an integer solution.
  x_rounded = x |> Nx.round()
  error = Nx.subtract(x, x_rounded) |> Nx.pow(2) |> Nx.sum() |> Nx.to_number()

  if error < epsilon do
    Nx.multiply(x_rounded, Nx.tensor([3, 1])) |> Nx.sum() |> Nx.to_number()
  else
    nil
  end
end

claw_machines
|> Enum.map(price)
|> Enum.filter(fn c -> c != nil end)
|> Enum.sum()
|> IO.inspect()
