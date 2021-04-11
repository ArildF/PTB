using System;
using System.Reactive;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace Rogue.Ptb.UI.Behaviors
{
	public class FocusBindingBehavior : Behavior<UIElement>
	{
		public static readonly DependencyProperty FocusProperty = DependencyProperty.Register(
			"Focus", typeof(IObservable<Unit>), typeof(FocusBindingBehavior), new PropertyMetadata(PropertyChangedCallback));

		private IDisposable _subscription;
		private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var behavior = (FocusBindingBehavior) d;
			behavior._subscription?.Dispose();

			var observable = (IObservable<Unit>) e.NewValue;
			behavior._subscription = observable.Subscribe(_ => behavior.AssociatedObject?.Focus());
		}

		public IObservable<Unit> Focus
		{
			get { return (IObservable<Unit>) GetValue(FocusProperty); }
			set { SetValue(FocusProperty, value); }
		}
		
	}
}