using Google.OrTools.Sat;

var input = File.ReadAllText("./input.txt").Trim();

if (Environment.GetEnvironmentVariable("part") == "part2")
	Console.WriteLine(input.Split('\n').Take(3).Select(ParseBlueprint).Aggregate(1L, (p, b) => p * CollectGeodes(b, 32)));
else
	Console.WriteLine(input.Split('\n').Select(ParseBlueprint).Select((b, i) => (i + 1L) * CollectGeodes(b, 24)).Sum());

static Dictionary<string, Dictionary<string, long>> ParseBlueprint(string input) =>
	String.Join(' ', input.Split(' ', StringSplitOptions.RemoveEmptyEntries)[2..]).Split('.', StringSplitOptions.RemoveEmptyEntries)
		.Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
		.ToDictionary(ws => ws[1],
			ws => ws.Zip(ws.Skip(1)).Where(p => p.Item1.All(Char.IsDigit)).ToDictionary(p => p.Item2, p => long.Parse(p.Item1)));

static long CollectGeodes(Dictionary<string, Dictionary<string, long>> blueprint, int minutes)
{
	var model = new CpModel();

	var buildBotDuring = blueprint.Keys.ToDictionary(k => k, _ => new List<LinearExpr>());
	foreach (var mineral in blueprint.Keys)
		for (int i = 0; i <= minutes; i++)
			buildBotDuring[mineral].Add(model.NewBoolVar($"{mineral}_{{{i}}}"));
	for (int i = 0; i < minutes; i++)
		model.Add(LinearExpr.Sum(blueprint.Keys.Select(m => buildBotDuring[m][i])) <= 1);

	var botsAfter = blueprint.Keys.ToDictionary(k => k, _ => new List<LinearExpr>());
	foreach (var mineral in blueprint.Keys)
		for (int i = 0; i <= minutes; i++)
			if (i == 0)
				botsAfter[mineral].Add((mineral == "ore" ? 1 : 0) + buildBotDuring[mineral][i]);
			else
				botsAfter[mineral].Add(botsAfter[mineral][^1] + buildBotDuring[mineral][i]);

	var countAfter = blueprint.Keys.ToDictionary(k => k, _ => new List<LinearExpr>());
	foreach (var mineral in blueprint.Keys)
		for (int i = 0; i <= minutes; i++)
		{
			var cost = LinearExpr.Sum(blueprint.Where(kvp => kvp.Value.ContainsKey(mineral)).Select(kvp => buildBotDuring[kvp.Key][i] * kvp.Value[mineral]));
			if (i <= 1)
				countAfter[mineral].Add((mineral == "ore" ? 1 : 0) - cost);
			else
				countAfter[mineral].Add(countAfter[mineral][^1] + botsAfter[mineral][i - 2] - cost);
			model.Add(countAfter[mineral][^1] >= 0);
		}

	model.Maximize(countAfter["geode"][^1]);
	var solver = new CpSolver();
	solver.Solve(model);
	return (long)solver.ObjectiveValue;
}
