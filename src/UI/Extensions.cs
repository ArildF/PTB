﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Threading;
using ReactiveUI;
using Rogue.Ptb.Infrastructure;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace Rogue.Ptb.UI
{
	public static class Extensions
	{
		public static void SetAndRaisePropertyChanged<TVm, TProperty>(this TVm self, 
			Expression<Func<TVm, TProperty>> func, 
			TProperty value,
			Action<TProperty> setter) where TVm : IReactiveObject
		{
			var currentValue = func.Compile()(self);

			if (EqualityComparer<TProperty>.Default.Equals(currentValue, value))
			{
				return;
			}

			self.RaisePropertyChanging(func.PropertyName());
			setter(value);
			self.RaisePropertyChanged(func.PropertyName());
		}

		public static void RaisePropertyChanging<TVm, TProperty>(this TVm self,
			Expression<Func<TVm, TProperty>> func)
			where TVm : IReactiveObject
		{
			self.RaisePropertyChanging(func.PropertyName());
		}

		public static void RaisePropertyChanged<TVm, TProperty>(this TVm self,
			Expression<Func<TVm, TProperty>> func)
			where TVm : IReactiveObject
		{
			self.RaisePropertyChanged(func.PropertyName());
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
			aggregator.Listen<T>().SubscribeOn(RxApp.MainThreadScheduler).Subscribe(handler);
		}

		// public static IObservable<IObservedChange<T, object>> PropertyOnAnyChanged<T, TRet>(
		// 	this IReactiveCollection<T> self, Expression<Func<T, TRet>> expression)
		// {
		// 	var name = expression.PropertyName();
		//
		// 	return self.ItemChanged.Where(t => t.PropertyName == name);
		// }

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

			public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
			{
				_dispatcher.Invoke(() => action(this, state), DispatcherPriority.ApplicationIdle);
				return NopDisposable.Instance;
			}

			public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
			{
				throw new NotImplementedException();
			}

			public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
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
