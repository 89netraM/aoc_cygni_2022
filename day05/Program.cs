Console.WriteLine(MoveCrates(Environment.GetEnvironmentVariable("part") == "part2" ? Part2 : Part1));

string MoveCrates(Action<int, Stack<char>, Stack<char>> mover)
{
	var parts = File.ReadAllText("./input.txt").Split("\n\n");
	var stacks = Transpose(parts[0].Split("\n")).Select(cs => new Stack<char>(cs.Where(Char.IsAsciiLetter).Reverse())).Where(cs => cs.Count > 0).ToArray();
	var moves = parts[1].Split("\n", StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split(" ").Where(w => w.All(Char.IsAsciiDigit)).Select(Int32.Parse).ToArray());

	foreach (var nums in moves)
	{
		mover(nums[0], stacks[nums[1] - 1], stacks[nums[2] - 1]);
	}

	return String.Concat(stacks.Select(s => s.Peek()));
}

void Part1(int count, Stack<char> from, Stack<char> to)
{
	for (int i = 0; i < count; i++)
	{
		to.Push(from.Pop());
	}
}

void Part2(int count, Stack<char> from, Stack<char> to)
{
	var temp = new Stack<char>();
	for (int i = 0; i < count; i++)
	{
		temp.Push(from.Pop());
	}
	for (int i = 0; i < count; i++)
	{
		to.Push(temp.Pop());
	}
}

static IEnumerable<IEnumerable<T>> Transpose<T>(IEnumerable<IEnumerable<T>> source)
{
	var enumerators = source.Select(e => e.GetEnumerator()).ToArray();
	while (true)
	{
		var row = new List<T>();
		foreach (var enumerator in enumerators)
		{
			if (enumerator.MoveNext())
			{
				row.Add(enumerator.Current);
			}
		}

		if (row.Count > 0)
		{
			yield return row;
		}
		else
		{
			break;
		}
	}
}
