var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2022.Day22.input.txt")!).ReadToEnd();
var parts = input.Split("\n\n");
var map = parts[0].Split('\n').SelectMany((l, y) => l.Select((c, x) => (p: (x, y), c))).Where(p => p.c != ' ').ToDictionary(p => p.p, p => p.c == '.');
var instructions = SplitParts(parts[1].Trim());

var pos = map.Keys.Where(p => p.y == 0).MinBy(p => p.x);
var dir = (x: 1, y: 0);

Func<((int, int), (int, int))> wrap = Environment.GetEnvironmentVariable("part") == "part2"
	? () => dir switch
		{
			(1, 0) when pos.y < 50 => ((99, 149 - (pos.y - 0)), (-1, 0)),
			(1, 0) when pos.y < 100 => ((100 + (pos.y - 50), 49), (0, -1)),
			(1, 0) when pos.y < 150 => ((149, 49 - (pos.y - 100)), (-1, 0)),
			(1, 0) when pos.y < 200 => ((50 + (pos.y - 150), 149), (0, -1)),
			(0, 1) when pos.x < 50 => ((100 + (pos.x - 0), 0), (0, 1)),
			(0, 1) when pos.x < 100 => ((49, 150 + (pos.x - 50)), (-1, 0)),
			(0, 1) when pos.x < 150 => ((99, 50 + (pos.x - 100)), (-1, 0)),
			(-1, 0) when pos.y < 50 => ((0, 100 + (49 - pos.y)), (1, 0)),
			(-1, 0) when pos.y < 100 => ((pos.y - 50, 100), (0, 1)),
			(-1, 0) when pos.y < 150 => ((50, 49 - (pos.y - 100)), (1, 0)),
			(-1, 0) when pos.y < 200 => ((pos.y - 100, 0), (0, 1)),
			(0, -1) when pos.x < 50 => ((50, 50 + pos.x), (1, 0)),
			(0, -1) when pos.x < 100 => ((0, pos.x + 100), (1, 0)),
			(0, -1) when pos.x < 150 => ((pos.x - 100, 199), (0, -1)),
		}
	: () => dir switch
		{
			(1, 0) => (map.Keys.Where(p => p.y == pos.y).MinBy(p => p.x), dir),
			(0, 1) => (map.Keys.Where(p => p.x == pos.x).MinBy(p => p.y), dir),
			(-1, 0) => (map.Keys.Where(p => p.y == pos.y).MaxBy(p => p.x), dir),
			(0, -1) => (map.Keys.Where(p => p.x == pos.x).MaxBy(p => p.y), dir),
		};

foreach (var instruction in instructions)
	if (int.TryParse(instruction.Span, out var steps))
		for (int s = 0; s < steps; s++)
		{
			var (nextPos, nextDir) = ((pos.x + dir.x, pos.y + dir.y), dir);
			if (!map.ContainsKey(nextPos))
				(nextPos, nextDir) = wrap();
			if (map[nextPos])
				(pos, dir) = (nextPos, nextDir);
			else
				break;
		}
	else
		if (instruction.Span[0] == 'R')
			dir = dir switch { (1, 0) => (0, 1), (0, 1) => (-1, 0), (-1, 0) => (0, -1), (0, -1) => (1, 0) };
		else
			dir = dir switch { (1, 0) => (0, -1), (0, -1) => (-1, 0), (-1, 0) => (0, 1), (0, 1) => (1, 0) };

Console.WriteLine(1000 * (pos.y + 1) + 4 * (pos.x + 1) + (dir switch { (1, 0) => 0, (0, 1) => 1, (-1, 0) => 2, (0, -1) => 3 }));

IEnumerable<ReadOnlyMemory<char>> SplitParts(string input)
{
	ReadOnlyMemory<char> memory = input.AsMemory();
	int start = 0;
	bool isDigit = Char.IsAsciiDigit(memory.Span[0]);
	for (int i = 1; i < memory.Length; i++)
		if (isDigit != (isDigit = Char.IsAsciiDigit(memory.Span[i])))
			yield return memory.Slice(start, -start + (start = i));
	yield return memory.Slice(start);
}
