using System;
using System.Collections;
using System.Collections.Generic;
using System.Concurrency;
using System.Linq.Expressions;
using System.Windows.Threading;
using ReactiveUI;
using Rogue.Ptb.Infrastructure;
using System.Linq;

namespace Rogue.Ptb.UI
{
	public static class Extensions
	{
		public static void SetAndRaisePropertyChanged<TVm, TProperty>(this TVm self, 
			Expression<Func<TVm, TProperty>> func, 
			TProperty value,
			Action<TProperty> setter) where TVm : IReactivePropertyChangingObject
		{
			var currentValue = func.Compile()(self);

			if (EqualityComparer<TProperty>.Default.Equals(currentValue, value))
			{
				return;
			}

			self.OnPropertyChanging(func.PropertyName());
			setter(value);
			self.OnPropertyChanged(func.PropertyName());
		}

		public static void RaisePropertyChanging<TVm, TProperty>(this TVm self,
			Expression<Func<TVm, TProperty>> func)
			where TVm : IReactivePropertyChangingObject
		{
			self.OnPropertyChanging(func.PropertyName());
		}

		public static void RaisePropertyChanged<TVm, TProperty>(this TVm self,
			Expression<Func<TVm, TProperty>> func)
			where TVm : IReactivePropertyChangingObject
		{
			self.OnPropertyChanged(func.PropertyName());
		}

		public static string PropertyName<T, TRet>(
					this Expression<Func<T, TRet>> expr)
		{
			if (expr.Body.NodeType == ExpressionType.MemberAccess)
			{
				return ((MemberExpression)expr.Body).Member.Name;
			}
			return null;
		}

		public static void ListenOnScheduler<T>(this IEventAggregator aggregator, Action<T> handler)
		{
			aggregator.Listen<T>().SubscribeOn(RxApp.DeferredScheduler).Subscribe(handler);
		}

		public static IObservable<IObservedChange<T, object>> PropertyOnAnyChanged<T, TRet>(
			this IReactiveCollection<T> self, Expression<Func<T, TRet>> expression)
		{
			var name = expression.PropertyName();

			return self.ItemChanged.Where(t => t.PropertyName == name);
		}

		public static IObservable<T> ObserveOnIdle<T>(this IObservable<T>  self)
		{
			return self.ObserveOn(new IdleDispatcher(Dispatcher.CurrentDispatcher));
		}

		private class IdleDispatcher : IScheduler
		{
			private readonly Dispatcher _dispatcher;

			public IdleDispatcher(Dispatcher dispatcher)
			{
				_dispatcher = dispatcher;
			}

			public IDisposable Schedule(Action action)
			{
				_dispatcher.Invoke(action, DispatcherPriority.ApplicationIdle);
				return NopDisposable.Instance;
			}

			public IDisposable Schedule(Action action, TimeSpan dueTime)
			{
				throw new NotImplementedException();
			}

			public DateTimeOffset Now
			{
				get { throw new NotImplementedException(); }
			}

			private class NopDisposable : IDisposable
			{
				public static IDisposable Instance = new NopDisposable();
				public void Dispose()
				{
					
				}
			}
		}
	}

}
