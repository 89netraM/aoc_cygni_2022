static class DictionaryExtensions
{
	public static Dictionary<Vector2, char> ToMap(this string input) =>
		input.Split('\n').SelectMany((l, y) => l.Select((c, x) => (p: new Vector2(x, y), c))).ToDictionary(p => p.p, p => p.c);

	public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue newValue, Func<TValue, TValue> updater)
	{
		if (dictionary.TryGetValue(key, out var value))
			dictionary[key] = updater(value);
		else
			dictionary[key] = newValue;
	}
}
