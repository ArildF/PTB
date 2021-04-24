using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Rogue.Ptb.Infrastructure
{
	public static class ObservableExtensions
	{
		public static IObservable<Unit> ToUnit<T>(this IObservable<T> self)
		{
			return self.Select(_ => Unit.Default);
		}
		
	}
}