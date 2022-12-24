public record struct Vector2(long X, long Y)
{
	public static Vector2 Up { get; } = new(0, -1);
	public static Vector2 Right { get; } = new(1, 0);
	public static Vector2 Left { get; } = new(-1, 0);
	public static Vector2 Down { get; } = new(0, 1);

	public IEnumerable<Vector2> NeighborsVonNeumann() =>
		new Vector2[] { new(X + 1, Y), new(X, Y - 1), new(X - 1, Y), new(X, Y + 1) };

	public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
}
