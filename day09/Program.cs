using System.Reflection;

var input = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2022.Day09.input.txt")).ReadToEnd();

Console.WriteLine(CountTailPositions(input, new Vector2[Environment.GetEnvironmentVariable("part") == "part2" ? 10 : 2]));

static long CountTailPositions(string input, Vector2[] rope)
{
	var positions = new HashSet<Vector2> { rope[^1] };

	foreach (var line in input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split(' ')))
	{
		var dir = line[0] switch
		{
			"R" => new Vector2(1, 0),
			"U" => new Vector2(0, -1),
			"L" => new Vector2(-1, 0),
			"D" => new Vector2(0, 1),
		};

		for (int i = 0; i < int.Parse(line[1]); i++)
		{
			rope[0] += dir;
			for (int t = 1; t < rope.Length; t++)
			{
				rope[t] = Follow(rope[t - 1], rope[t]);
			}
			positions.Add(rope[^1]);
		}
	}

	return positions.Count;
}

static Vector2 Follow(Vector2 head, Vector2 tail)
{
	if (tail.Distance(head) > 1.5d)
	{
		var primary = head.NeighborsVonNeumann().Where(n => tail.Distance(n) < 1.5d).OrderBy(n => tail.Distance(n));
		if (primary.Any())
		{
			tail = primary.First();
		}
		else
		{
			tail = head.NeighborsMoore().Where(n => tail.Distance(n) < 1.5d).OrderBy(n => tail.Distance(n)).First();
		}
	}
	return tail;
}

readonly record struct Vector2(long X, long Y)
{
	public double Distance(Vector2 o) => Math.Sqrt((o.X - X) * (o.X - X) + (o.Y - Y) * (o.Y - Y));
	public IEnumerable<Vector2> NeighborsVonNeumann() => new Vector2[] { new(X, Y - 1), new(X - 1, Y), new(X + 1, Y), new(X, Y + 1) };
	public IEnumerable<Vector2> NeighborsMoore() => new Vector2[] { new(X, Y - 1), new(X + 1, Y - 1), new(X - 1, Y), new(X - 1, Y - 1), new(X + 1, Y), new(X + 1, Y + 1), new(X, Y + 1), new(X - 1, Y + 1) };
	public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
}
