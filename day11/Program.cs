using System.Linq.Expressions;
using RegExtract;

var monkeys = File.ReadAllText("./input.txt").Split("\n\n").Select(ParseMonkey).ToArray();

var (iterations, div, mod) = Environment.GetEnvironmentVariable("part") == "part2" ? (10000, 1, monkeys.Select(m => m.Test).Aggregate(Lcm)) : (20, 3, long.MaxValue);

var business = new long[monkeys.Length];
for (int i = 0; i < iterations; i++)
{
	for (int m = 0; m < monkeys.Length; m++)
	{
		while (monkeys[m].Items.Count > 0)
		{
			var item = monkeys[m].Items.Pop();

			item = (monkeys[m].Operation(item) / div) % mod;

			monkeys[((item % monkeys[m].Test) == 0) ? monkeys[m].TrueTarget : monkeys[m].FalseTarget].Items.Push(item);

			business[m]++;
		}
	}
}

Console.WriteLine(business.OrderDescending().Take(2).Aggregate((a, b) => a * b));

static Monkey ParseMonkey(string input)
{
	var lines = input.Split("\n", StringSplitOptions.None);
	return new Monkey(
		lines[1].Extract<List<long>>(@"(?:(\d+)(?:, )?)+").Aggregate(new Stack<long>(), (s, v) => { s.Push(v); return s; }),
		ParseOperation(lines[2]),
		lines[3].Extract<long>(@"(\d+)"),
		lines[4].Extract<int>(@"(\d+)"),
		lines[5].Extract<int>(@"(\d+)"));
}

static Func<long, long> ParseOperation(string input)
{
	var param = Expression.Parameter(typeof(long), "old");
	Expression right = long.TryParse(input[25..], out long c) ? Expression.Constant(c, typeof(long)) : param;
	var op = input[23] == '*' ? Expression.Multiply(param, right) : Expression.Add(param, right);
	return Expression.Lambda<Func<long, long>>(op, new[] { param }).Compile();
}

static long Lcm(long a, long b) => Math.Abs(a * b) / Gcd(a, b);
static long Gcd(long a, long b)
{
	while (b != 0L)
	{
		(a, b) = (b, a % b);
	}
	return Math.Abs(a);
}

record Monkey(Stack<long> Items, Func<long, long> Operation, long Test, int TrueTarget, int FalseTarget);
