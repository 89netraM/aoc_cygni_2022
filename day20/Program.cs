var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2022.Day20.input.txt")!).ReadToEnd().Trim();
var part2 = Environment.GetEnvironmentVariable("part") == "part2";

var list = new LinkedList<long>(input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse));

var order = new List<LinkedListNode<long>>(list.Count);
LinkedListNode<long> zero = null;
for (var node = list.First; node is not null; node = node.Next)
{
	order.Add(node);
	if (node.Value == 0)
		zero = node;
	if (part2)
		node.Value *= 811589153L;
}

for (int i = 0; i < (part2 ? 10 : 1); i++)
	foreach (var node in order)
	{
		var before = node.Previous ?? list.Last;
		list.Remove(node);
		var moves = (node.Value % list.Count + list.Count) % list.Count;
		for (long m = 0; m < moves; m++)
			before = before.Next ?? list.First;
		list.AddAfter(before, node);
	}

var sum = 0L;
for (int i = 0; i <= 3000; i++, zero = zero.Next ?? list.First)
	if (i is 1000 or 2000 or 3000)
		sum += zero.Value;

Console.WriteLine(sum);
