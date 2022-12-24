using System.Collections.Immutable;

var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2022.Day24.input.txt")!).ReadToEnd().Trim();

var inputMap = input.ToMap().Where(kvp => kvp.Value != '#');

var start = inputMap.MinBy(kvp => kvp.Key.Y).Key;
var goal = inputMap.MaxBy(kvp => kvp.Key.Y).Key;

var maps = new List<ImmutableDictionary<Vector2, Tile>>();
maps.Add(inputMap.Where(kvp => kvp.Value != '.').ToImmutableDictionary(kvp => kvp.Key, kvp => ToTile(kvp.Value)));
var min = start + Vector2.Down;
var max = goal + Vector2.Up;

BFS.Search(
	(pos: start, map: 0),
	MakeMoves,
	p => p.pos == goal,
	out var toGoal);
if (Environment.GetEnvironmentVariable("part") != "part2")
{
	Console.WriteLine(toGoal.Count());
	return;
}
BFS.Search(
	(pos: goal, map: toGoal.Last().map),
	MakeMoves,
	p => p.pos == start,
	out var backToStart);
BFS.Search(
	(pos: start, map: backToStart.Last().map),
	MakeMoves,
	p => p.pos == goal,
	out var backToGoal);
Console.WriteLine(toGoal.Count() + backToStart.Count() + backToGoal.Count());

IEnumerable<(Vector2, int)> MakeMoves((Vector2, int) state)
{
	var (pos, map) = state;
	var nextMap = GetMap(map + 1);
	foreach (var nextPos in pos.NeighborsVonNeumann().Append(pos))
	{
		if ((IsInBounds(nextPos) || nextPos == start || nextPos == goal) && !nextMap.ContainsKey(nextPos))
		{
			yield return (nextPos, map + 1);
		}
	}
}

ImmutableDictionary<Vector2, Tile> GetMap(int mapIndex)
{
	if (mapIndex < maps.Count)
		return maps[mapIndex];

	for (int i = maps.Count - 1; i <= mapIndex; i++)
	{
		var map = maps[i];
		var mapBuilder = ImmutableDictionary.CreateBuilder<Vector2, Tile>();
		foreach (var (pos, tiles) in map)
			foreach (var tile in Enum.GetValues<Tile>())
				if (tiles.HasFlag(tile))
				{
					var nextPos = pos + Direction(tile);
					if (!IsInBounds(nextPos))
						nextPos = tile switch
						{
							Tile.Up    => new Vector2(pos.X, max.Y),
							Tile.Right => new Vector2(min.X, pos.Y),
							Tile.Left  => new Vector2(max.X, pos.Y),
							Tile.Down  => new Vector2(pos.X, min.Y),
						};
					mapBuilder.AddOrUpdate(nextPos, tile, t => t | tile);
				}
		maps.Add(mapBuilder.ToImmutable());
	}
	return maps[mapIndex];
}

bool IsInBounds(Vector2 pos) => min.X <= pos.X && pos.X <= max.X && min.Y <= pos.Y && pos.Y <= max.Y;

Vector2 Direction(Tile tile) => tile switch
{
	Tile.Up    => Vector2.Up,
	Tile.Right => Vector2.Right,
	Tile.Left  => Vector2.Left,
	Tile.Down  => Vector2.Down,
};

Tile ToTile(char c) => c switch
{
	'^' => Tile.Up,
	'>' => Tile.Right,
	'<' => Tile.Left,
	'v' => Tile.Down,
};

[Flags]
enum Tile
{
	Up    = 0b0001,
	Right = 0b0010,
	Left  = 0b0100,
	Down  = 0b1000,
}
