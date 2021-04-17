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
	}
}