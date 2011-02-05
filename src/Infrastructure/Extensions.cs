using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.Ptb.Infrastructure
{
	public static class Extensions
	{
		public static IEnumerable<T> TraverseBy<T>(this T self, Func<T, T> traversal) where T:class
		{
			var next = self;
			yield return next;

			while (true)
			{
				next = traversal(next);
				if (next == null)
				{
					yield break;
				}
				yield return next;
			}

		}

		public static bool In<T>(this T self, params T[] candidates)
		{
			return candidates.Any(c => c.Equals(self));
		}

		public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
		{
			foreach (var item in self)
			{
				action(item);
			}
		}
	}
}
