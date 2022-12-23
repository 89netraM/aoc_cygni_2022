var Moves = new ((long x, long y), (long x, long y), (long x, long y))[]
{
	((-1, -1), (0, -1), (1, -1)),
	((1, 1), (0, 1), (-1, 1)),
	((-1, -1), (-1, 0), (-1, 1)),
	((1, 1), (1, 0), (1, -1)),
};

var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2022.Day23.input.txt")!).ReadToEnd().Trim();
var part2 = Environment.GetEnvironmentVariable("part") == "part2";
var map = input.Split('\n').SelectMany((l, y) => l.Select((c, x) => (p: (x: (long)x, y: (long)y), c))).Where(p => p.c == '#').Select(p => p.p).ToHashSet();
var map2 = new HashSet<(long, long)>(map.Count);
var elfDirection = new Dictionary<(long, long), (long x, long y)>(map.Count);
var proposed = new Dictionary<(long, long), long>(map.Count);

for (int round = 0; round < 10 || part2; round++)
{
	foreach (var elf in map)
		if ((new (long x, long y)[] { (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1), (0, 1), (1, 1) }).Any(d => map.Contains((elf.x + d.x, elf.y + d.y))))
			for (int move = 0; move < Moves.Length; move++)
			{
				var (left, forward, right) = Moves[(round + move) % Moves.Length];
				if (!map.Contains((elf.x + left.x, elf.y + left.y)) && !map.Contains((elf.x + forward.x, elf.y + forward.y)) && !map.Contains((elf.x + right.x, elf.y + right.y)))
				{
					proposed[(elf.x + forward.x, elf.y + forward.y)] = proposed.GetValueOrDefault((elf.x + forward.x, elf.y + forward.y)) + 1;
					elfDirection.Add(elf, forward);
					break;
				}
			}

	if (part2 && proposed.Count == 0)
	{
		Console.WriteLine(round + 1);
		return;
	}

	foreach (var elf in map)
		if (elfDirection.TryGetValue(elf, out var dir) && proposed[(elf.x + dir.x, elf.y + dir.y)] == 1)
			map2.Add((elf.x + dir.x, elf.y + dir.y));
		else
			map2.Add(elf);

	(map, map2) = (map2, map);
	map2.Clear();
	elfDirection.Clear();
	proposed.Clear();
}

var min = map.Aggregate((a, b) => (Math.Min(a.x, b.x), Math.Min(a.y, b.y)));
var max = map.Aggregate((a, b) => (Math.Max(a.x, b.x), Math.Max(a.y, b.y)));
var area = (max.x - min.x + 1) * (max.y - min.y + 1);
Console.WriteLine(area - map.Count);
