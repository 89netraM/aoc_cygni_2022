var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2022.Day12.input.txt")).ReadToEnd();

var map = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).SelectMany((l, y) => l.Select((c, x) => (x, y, c))).ToDictionary(t => (t.x, t.y), t => t.c);
var start = map.Single(kvp => kvp.Value == 'S').Key;
map[start] = 'a';
var end = map.Single(kvp => kvp.Value == 'E').Key;
map[end] = 'z';

if (Environment.GetEnvironmentVariable("part") == "part2")
	Console.WriteLine(Search(end, (c, n) => map.ContainsKey(n) && (map[c] - map[n]) <= 1, c => map[c] == 'a'));
else
	Console.WriteLine(Search(start, (c, n) => map.ContainsKey(n) && (map[n] - map[c]) <= 1, c => c == end));

static int Search((int, int) start, Func<(int, int), (int, int), bool> isNext, Predicate<(int, int)> goalCondition)
{
	var distanceTo = new HashSet<(int, int)> { start };
	var toVisit = new PriorityQueue<(int x, int y), int>();
	toVisit.Enqueue(start, 0);

	while (toVisit.TryDequeue(out var curr, out var dist))
	{
		if (goalCondition(curr))
			return dist;

		foreach (var next in new[] { (curr.x, curr.y - 1), (curr.x - 1, curr.y), (curr.x + 1, curr.y), (curr.x, curr.y + 1) })
			if (isNext(curr, next) && distanceTo.Add(next))
				toVisit.Enqueue(next, dist + 1);
	}

	return -1;
}
