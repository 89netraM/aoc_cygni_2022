using System.Collections.Immutable;
using Priority_Queue;

public static class BFS
{
	public static bool Search<TNode>(
		TNode start,
		Func<TNode, IEnumerable<TNode>> getNext,
		Func<TNode, bool> goalCondition,
		out IEnumerable<TNode> path
	) where TNode : notnull
	{
		IDictionary<TNode, (IImmutableList<TNode>, long)> directionTo = new Dictionary<TNode, (IImmutableList<TNode>, long)>();
		directionTo.Add(start, (ImmutableList.Create<TNode>(), 0));
		IPriorityQueue<TNode, long> toVisit = new SimplePriorityQueue<TNode, long>();
		toVisit.Enqueue(start, 0);

		while (toVisit.Count > 0)
		{
			TNode current = toVisit.Dequeue();
			var (pathToCurrent, cost) = directionTo[current];
			if (goalCondition(current))
			{
				path = pathToCurrent;
				return true;
			}
			long nextCost = cost + 1;
			foreach (var next in getNext(current))
			{
				if (!directionTo.TryGetValue(next, out var pathToNextAndCost) || nextCost < pathToNextAndCost.Item2)
				{
					directionTo[next] = (pathToCurrent.Add(next), nextCost);
					if (toVisit.Contains(next))
					{
						toVisit.UpdatePriority(next, nextCost);
					}
					else
					{
						toVisit.Enqueue(next, nextCost);
					}
				}
			}
		}

		path = Enumerable.Empty<TNode>();
		return false;
	}
}
