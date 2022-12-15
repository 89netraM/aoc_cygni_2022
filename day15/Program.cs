var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2022.Day15.input.txt")).ReadToEnd();
var sensors = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(l => Matcher.regex().Match(l))
	.Select(m => (s: (x: long.Parse(m.Groups[1].Value), y: long.Parse(m.Groups[2].Value)), b: (x: long.Parse(m.Groups[3].Value), y: long.Parse(m.Groups[4].Value))));

if (Environment.GetEnvironmentVariable("part") == "part2")
{
	var sensDist = sensors.Select(p => (p.s, d: ManhattanDistance(p.s, p.b))).ToArray();
	foreach (var (s, d) in sensDist)
	{
		var edge = d + 1;
		for (long x = -edge; x <= edge; x++)
			foreach (var point in new (long x, long y)[] { (s.x + x, s.y + (edge - Math.Abs(x))), (s.x + x, s.y - (edge - Math.Abs(x))) })
				if (0 <= point.x && point.x <= 4000000 && 0 <= point.y && point.y <= 4000000 && sensDist.All(p => ManhattanDistance(p.s, point) > p.d))
				{
					Console.WriteLine(point.x * 4000000 + point.y);
					return;
				}
	}
}
else
{
	var nonBeacons = new HashSet<long>();

	foreach (var (s, b) in sensors)
	{
		var distance = ManhattanDistance(s, b);
		var y = Math.Abs(2000000 - s.y);
		if (y < distance)
		{
			var xMin = s.x - (distance - y);
			var xMax = s.x + (distance - y);
			for (long x = xMin; x <= xMax; x++)
			{
				if (!(b.y == 2000000 && b.x == x))
				{
					nonBeacons.Add(x);
				}
			}
		}
	}

	Console.WriteLine(nonBeacons.Count);
}

static long ManhattanDistance((long x, long y) a, (long x, long y) b) => Math.Abs(b.x - a.x) + Math.Abs(b.y - a.y);

public static partial class Matcher
{
	[System.Text.RegularExpressions.GeneratedRegex(@"x=(-?\d+), y=(-?\d+).*?x=(-?\d+), y=(-?\d+)")]
	public static partial System.Text.RegularExpressions.Regex regex();
}
