using System.Reflection;

var input = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2022.Day10.input.txt")).ReadToEnd();

long x = 1;
if (Environment.GetEnvironmentVariable("part") == "part2")
{
	long c = 0;
	var screen = new HashSet<long>();
	void MaybeAdd() { if (c % 40 == x - 1 || c % 40 == x || c % 40 == x + 1) { screen.Add(c); } }
	foreach (var inst in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
	{
		if (inst == "noop")
		{
			MaybeAdd();
			c++;
		}
		else
		{
			MaybeAdd();
			c++;
			MaybeAdd();
			x += long.Parse(inst.Substring(4));
			c++;
		}
	}
	MaybeAdd();
	for (long row = 0; row < 6; row++)
	{
		for (long col = 0; col < 40; col++)
		{
			Console.Write(screen.Contains(row * 40 + col) ? '#' : '.');
		}
		Console.WriteLine();
	}
}
else
{
	long c = 1;
	long signal = 0;
	void MaybeInc() { if (new long[] { 20, 60, 100, 140, 180, 220 }.Contains(c)) { signal += c * x; } }
	foreach (var inst in input.Split('\n', StringSplitOptions.RemoveEmptyEntries))
	{
		if (inst == "noop")
		{
			c++;
			MaybeInc();
		}
		else
		{
			c++;
			MaybeInc();
			x += long.Parse(inst.Substring(4));
			c++;
			MaybeInc();
		}
	}
	Console.WriteLine(signal);
}
