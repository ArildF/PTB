using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace Rogue.Ptb.Infrastructure
{
	public static class Extensions
	{
		public static IEnumerable<T> TraverseBy<T>(this T self, Func<T, T> traversal, bool log = false) where T:class
		{
			Debug.WriteLineIf(log, $"Traversing, starting from {self}");
			var next = self;
			yield return next;

			while (true)
			{
				next = traversal(next);
				if (next == null)
				{
					Debug.WriteLineIf(log, "      <found null>");
					yield break;
				}
				Debug.WriteLineIf(log, $"      {next}");
				yield return next;
			}

		}

		public static bool In<T>(this T self, params T[] candidates)
		{
			return candidates.Any(c => c.Equals(self));
		}

		//public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
		//{
		//    foreach (var item in self)
		//    {
		//        action(item);
		//    }
		//}

		public static void DoIfNotNull<T>(this T self, Action<T> action) where T : class
		{
			if (self != null)
			{
				action(self);
			}
		}

		public static TRet IfNotNull<T, TRet>(this T self, Func<T, TRet> func) where T : class
		{
			if (self != null)
			{
				return func(self);
			}
			return default(TRet);
		}

		public static async System.Threading.Tasks.Task PublishAndWait<TSend, TAnswer>(this IEventAggregator bus, 
			TSend message)
		{
			var semaphore = new SemaphoreSlim(0, 1);
			IDisposable subscription = null;
			subscription = bus.Listen<TAnswer>()
				.ObserveOn(TaskPoolScheduler.Default)
				.Subscribe(_ =>
				{
					semaphore.Release();
					subscription?.Dispose();
				});
			bus.Publish(message);
			await semaphore.WaitAsync();
		}
	}
}
