using System;

namespace AoC.Library;

public static class StringUtils
{
	private static readonly string[] lineSeparators = new[] { "\r\n", "\n" };

	public static string[] Lines(this string s) =>
		s.Split(lineSeparators, StringSplitOptions.RemoveEmptyEntries);
}
