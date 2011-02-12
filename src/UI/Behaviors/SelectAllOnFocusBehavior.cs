using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Rogue.Ptb.UI.Behaviors
{
	public class SelectAllOnFocusBehavior : Behavior<TextBoxBase>
	{
		protected override void OnAttached()
		{
			AssociatedObject.GotKeyboardFocus += AssociatedObjectOnGotKeyboardFocus;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.GotKeyboardFocus -= AssociatedObjectOnGotKeyboardFocus;
		}

		private void AssociatedObjectOnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			AssociatedObject.SelectAll();
		}
	}
}
