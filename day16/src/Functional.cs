using System;

namespace AoC.Library;

public static class Functional
{
	public static Func<TA1, TA2, TA3, TR> Curry<TA1, TA2, TA3, TR>(Func<(TA1, TA2, TA3), TR> f) =>
		(a1, a2, a3) => f((a1, a2, a3));
	public static Func<(TA1, TA2, TA3), TR> Uncurry<TA1, TA2, TA3, TR>(Func<TA1, TA2, TA3, TR> f) =>
		p => f(p.Item1, p.Item2, p.Item3);

	public static Func<TA, TR> Memoize<TA, TR>(Func<TA, TR> f) where TA : notnull =>
		new Memoization<TA, TR>(f);
}

