var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2022.Day18.input.txt")!).ReadToEnd().Trim();
var lava = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split(',').Select(long.Parse).ToArray()).Select(s => (x: s[0], y: s[1], z: s[2])).ToHashSet();

var totalSides = lava.Sum(d => Neighbors(d).Count(n => !lava.Contains(n)));

if (Environment.GetEnvironmentVariable("part") != "part2")
{
	Console.WriteLine(totalSides);
	return;
}

var min = lava.Aggregate((a, b) => (Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z)));
var max = lava.Aggregate((a, b) => (Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z)));

var toVisit = new Queue<(long x, long y, long z)>(); toVisit.Enqueue(min);
while (toVisit.TryDequeue(out var curr))
	if (min.x <= curr.x && curr.x <= max.x && min.y <= curr.y && curr.y <= max.y && min.z <= curr.z && curr.z <= max.z && lava.Add(curr))
		Neighbors(curr).ForEach(toVisit.Enqueue);

var insides = lava.Sum(d => Neighbors(d).Count(n => min.x < n.x && n.x < max.x && min.y < n.y && n.y < max.y && min.z < n.z && n.z < max.z && !lava.Contains(n)));

Console.WriteLine(totalSides - insides);

static List<(long x, long y, long z)> Neighbors((long x, long y, long z) pos) => new()
{
	(pos.x + 1, pos.y, pos.z), (pos.x - 1, pos.y, pos.z),
	(pos.x, pos.y + 1, pos.z), (pos.x, pos.y - 1, pos.z),
	(pos.x, pos.y, pos.z + 1), (pos.x, pos.y, pos.z - 1),
};
