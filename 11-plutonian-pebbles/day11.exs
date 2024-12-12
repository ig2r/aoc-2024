{:ok, data} = File.read("input.txt")

pebble_counts =
  data
  |> String.trim()
  |> String.split(" ")
  |> Map.from_keys(1)

transmogrify = fn
  "0" ->
    ["1"]

  pebble ->
    l = String.length(pebble)

    if rem(l, 2) == 0 do
      {p1, p2} = String.split_at(pebble, div(l, 2))
      p2_stripped_zeroes = p2 |> String.to_integer() |> Integer.to_string()
      [p1, p2_stripped_zeroes]
    else
      [(2024 * String.to_integer(pebble)) |> Integer.to_string()]
    end
end

blink_once = fn pebble_counts when is_map(pebble_counts) ->
  pebble_counts
  |> Enum.flat_map(fn {p, c} -> Enum.map(transmogrify.(p), fn t -> {t, c} end) end)
  |> Enum.reduce(Map.new(), fn {t, c}, acc -> Map.update(acc, t, c, fn v -> v + c end) end)
end

blink_times = fn pebble_counts, n when is_map(pebble_counts) and is_integer(n) ->
  Enum.reduce(1..n, pebble_counts, fn _, acc -> blink_once.(acc) end)
end

sum_pebbles = fn pebble_counts when is_map(pebble_counts) ->
  pebble_counts |> Enum.reduce(0, fn {_, c}, acc -> acc + c end)
end

result_part1 = sum_pebbles.(blink_times.(pebble_counts, 25))
result_part2 = sum_pebbles.(blink_times.(pebble_counts, 75))

IO.puts("Part 1: #{result_part1}")
IO.puts("Part 1: #{result_part2}")
