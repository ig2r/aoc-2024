require Integer

{:ok, data} = File.read("input.txt")

pebble_counts =
  data
  |> String.split()
  |> Enum.map(&String.to_integer/1)
  |> Map.from_keys(1)

transmogrify = fn
  0 ->
    [1]

  pebble ->
    digits = Integer.digits(pebble)

    if Integer.is_even(length(digits)) do
      {p1, p2} = Enum.split(digits, div(length(digits), 2))
      [p1, p2] |> Enum.map(&Integer.undigits/1)
    else
      [2024 * pebble]
    end
end

blink_once = fn pebble_counts ->
  pebble_counts
  |> Enum.flat_map(fn {p, c} -> Enum.map(transmogrify.(p), fn t -> {t, c} end) end)
  |> Enum.reduce(%{}, fn {t, c}, acc -> Map.update(acc, t, c, fn v -> v + c end) end)
end

blink_times = fn pebble_counts, n ->
  Enum.reduce(1..n, pebble_counts, fn _, acc -> blink_once.(acc) end)
end

sum_pebbles = fn pebble_counts ->
  pebble_counts |> Map.values() |> Enum.sum()
end

result_part1 = pebble_counts |> blink_times.(25) |> sum_pebbles.()
result_part2 = pebble_counts |> blink_times.(75) |> sum_pebbles.()

IO.puts("Part 1: #{result_part1}")
IO.puts("Part 2: #{result_part2}")
