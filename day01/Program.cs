Console.WriteLine(Environment.GetEnvironmentVariable("part") == "part2" ? Part2() : Part1());

long Part1() => SumElf().Max();

long Part2() => SumElf().OrderDescending().Take(3).Sum();

IEnumerable<long> SumElf() => File.ReadAllText("./input.txt").Split("\n\n").Select(e => e.Split("\n", StringSplitOptions.RemoveEmptyEntries).Sum(Int64.Parse));
