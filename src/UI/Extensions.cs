using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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

	}

}
