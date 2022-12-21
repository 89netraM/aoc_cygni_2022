var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2022.Day21.input.txt")!).ReadToEnd().Trim();
var monkeys = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(l => l.Split(": ")).ToDictionary(p => p[0], p => p[1].Split(' '));

var result = ParseExpr("root", Environment.GetEnvironmentVariable("part") == "part2");
Console.WriteLine(((Value)result).value);

IExpr ParseExpr(string monkey, bool part2) =>
	(part2 && monkey == "humn")
		? new Var()
		: (monkeys[monkey].Length == 1)
			? new Value(long.Parse(monkeys[monkey][0]))
			: (ParseExpr(monkeys[monkey][0], part2), monkeys[monkey][1], ParseExpr(monkeys[monkey][2], part2)) switch
			{
				(var l, _, Value(var r)) when part2 && monkey == "root" => Solve(l, r),
				(Value(var l), "+", Value(var r)) => new Value(l + r),
				(Value(var l), "-", Value(var r)) => new Value(l - r),
				(Value(var l), "*", Value(var r)) => new Value(l * r),
				(Value(var l), "/", Value(var r)) => new Value(l / r),
				(var l, var op, var r) => new Expr(l, op, r),
			};

IExpr Solve(IExpr left, long right) =>
	left switch
	{
		Expr(var e, "+", Value(var c)) => Solve(e, right - c),
		Expr(Value(var c), "+", var e) => Solve(e, right - c),
		Expr(var e, "-", Value(var c)) => Solve(e, right + c),
		Expr(Value(var c), "-", var e) => Solve(e, c - right),
		Expr(var e, "*", Value(var c)) => Solve(e, right / c),
		Expr(Value(var c), "*", var e) => Solve(e, right / c),
		Expr(var e, "/", Value(var c)) => Solve(e, right * c),
		Expr(Value(var c), "/", var e) => Solve(e, c / right),
		Var => new Value(right),
	};

interface IExpr { }
record Var() : IExpr;
record Value(long value) : IExpr;
record Expr(IExpr left, string op, IExpr right) : IExpr;
