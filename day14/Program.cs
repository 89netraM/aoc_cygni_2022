var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2022.Day14.input.txt")).ReadToEnd();

var part2 = Environment.GetEnvironmentVariable("part") == "part2";

var walls = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
	.SelectMany(ParseWall)
	.ToHashSet();
var floor = walls.Max(p => p.y) + 2;
long sandCount = 0;

while (true)
{
	var sand = (x: 500, y: 0);
	if (walls.Contains(sand))
	{
		Console.WriteLine(sandCount);
		return;
	}
	while (true)
	{
		if (sand.y > floor)
		{
			Console.WriteLine(sandCount);
			return;
		}

		var next = (x: sand.x, y: sand.y + 1);
		if (part2 && next.y == floor)
			break;
		if (!walls.Contains(next))
		{
			sand = next;
			continue;
		}
		next = (next.x - 1, next.y);
		if (!walls.Contains(next))
		{
			sand = next;
			continue;
		}
		next = (next.x + 2, next.y);
		if (!walls.Contains(next))
		{
			sand = next;
			continue;
		}
		break;
	}
	walls.Add(sand);
	sandCount += 1;
}

static IEnumerable<(long x, long y)> ParseWall(string input)
{
	var ps = input.Split(" -> ").Select(s => s.Split(',').Select(long.Parse).ToArray()).Select(p => (x: p[0], y: p[1])).ToArray();
	var p = ps[0];
	for (int i = 1; i < ps.Length; i++)
		for (var d = (x: Math.Sign(ps[i].x - p.x), y: Math.Sign(ps[i].y - p.y)); p != ps[i]; p = (p.x + d.x, p.y + d.y))
			yield return p;
	yield return p;
}
