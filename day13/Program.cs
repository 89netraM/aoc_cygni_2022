using System.Text.Json;

var input = File.ReadAllText("./input.txt");

if (Environment.GetEnvironmentVariable("part") == "part2")
{
	var two = JsonSerializer.Deserialize<JsonElement>("[[2]]");
	var six = JsonSerializer.Deserialize<JsonElement>("[[6]]");
	var packets = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(Parse).Append(two).Append(six).ToList();
	packets.Sort(Compare);
	Console.WriteLine((packets.IndexOf(two) + 1) * (packets.IndexOf(six) + 1));
}
else
	Console.WriteLine(input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries).Select((s, i) => (i, s: s.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(Parse).ToArray())).Where(p => Compare(p.s[0], p.s[1]) <= 0).Sum(p => p.i + 1));

static int Compare(JsonElement a, JsonElement b) =>
	(a.ValueKind, b.ValueKind) switch
	{
		(JsonValueKind.Number, JsonValueKind.Number) => a.GetInt64().CompareTo(b.GetInt64()),
		(JsonValueKind.Number, JsonValueKind.Array) => Compare(JsonSerializer.SerializeToElement(new[] { a.GetInt64() }), b),
		(JsonValueKind.Array, JsonValueKind.Number) => Compare(a, JsonSerializer.SerializeToElement(new[] { b.GetInt64() })),
		(JsonValueKind.Array, JsonValueKind.Array) => a.EnumerateArray().Zip(b.EnumerateArray()).Aggregate(0, (r, p) => r != 0 ? r : Compare(p.First, p.Second)) switch
		{
			0 => a.GetArrayLength().CompareTo(b.GetArrayLength()),
			int c => c,
		},
	};

static JsonElement Parse(string input) =>
	JsonSerializer.Deserialize<JsonElement>(input);
