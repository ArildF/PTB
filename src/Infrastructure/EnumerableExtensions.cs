using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Ptb.Infrastructure
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> AsSingleItemEnumerable<T>(this T self)
		{
			yield return self;
		}

		public static IEnumerable<T> RecurseDepthFirst<T>(this T self,
			Func<T, IEnumerable<T>> recurse)
		{
			yield return self;
			foreach (var subItem in recurse(self).SelectMany(i => RecurseDepthFirst(i, recurse)))
			{
				yield return subItem;
			}
		}

		public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> self) where T : class
		{
			return self.Where(t => t != null)!;
		}

		public static IEnumerable<T?> AsNullable<T>(this IEnumerable<T> self) where T : struct
		{
			return self.Select(t => (T?) t);
		}
	}
}