var input = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("AoC.Year2022.Day25.input.txt")!).ReadToEnd().Trim();
Console.WriteLine(input.Split('\n').Aggregate(Add));

string Add(string a, string b)
{
	int length = Math.Max(a.Length, b.Length);
	var aChars = a.Reverse().Concat(Enumerable.Repeat('0', int.MaxValue)).Take(length);
	var bChars = b.Reverse().Concat(Enumerable.Repeat('0', int.MaxValue)).Take(length);

	var result = new Stack<char>();
	char remainder = '0';
	foreach (var (aC, bC) in aChars.Zip(bChars))
	{
		(remainder, var res) = AddDigitRem(aC, bC, remainder);
		result.Push(res);
	}
	result.Push(remainder);
	while (result.Peek() == '0') result.Pop();

	return String.Concat(result);
}

(char, char) AddDigitRem(char a, char b, char remainder)
{
	var (rem1, res) = AddDigit(a, b);
	(var rem2, res) = AddDigit(res, remainder);
	var (_, rem) = AddDigit(rem1, rem2);
	return (rem, res);
}

(char, char) AddDigit(char a, char b) =>
	(a, b) switch
	{
		('=', '=') => ('-', '1'),
		('=', '-') => ('-', '2'),
		('=', '0') => ('0', '='),
		('=', '1') => ('0', '-'),
		('=', '2') => ('0', '0'),
		('-', '=') => ('-', '2'),
		('-', '-') => ('0', '='),
		('-', '0') => ('0', '-'),
		('-', '1') => ('0', '0'),
		('-', '2') => ('0', '1'),
		('0', '=') => ('0', '='),
		('0', '-') => ('0', '-'),
		('0', '0') => ('0', '0'),
		('0', '1') => ('0', '1'),
		('0', '2') => ('0', '2'),
		('1', '=') => ('0', '-'),
		('1', '-') => ('0', '0'),
		('1', '0') => ('0', '1'),
		('1', '1') => ('0', '2'),
		('1', '2') => ('1', '='),
		('2', '=') => ('0', '0'),
		('2', '-') => ('0', '1'),
		('2', '0') => ('0', '2'),
		('2', '1') => ('1', '='),
		('2', '2') => ('1', '-'),
	};
