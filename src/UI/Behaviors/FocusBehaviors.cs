using System;
using System.Windows;

namespace Rogue.Ptb.UI.Behaviors
{
	public class FocusBehaviors
	{
		public static readonly DependencyProperty IsFocusedProperty = DependencyProperty.RegisterAttached("IsFocused",
			typeof(bool), typeof(FocusBehaviors), new PropertyMetadata(DefaultValueChanged));

		public static bool GetIsFocused(UIElement dependencyObject)
		{
			return (bool) dependencyObject.GetValue(IsFocusedProperty);
		}

		public static void SetIsFocused(UIElement dependencyObject, bool value)
		{
			dependencyObject.SetValue(IsFocusedProperty, value);
		}

		private static void DefaultValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			var element = (UIElement) sender;

			var newValue = (bool) e.NewValue;
			if (newValue)
			{
				element.Focus();
			}
		}
	}
}
