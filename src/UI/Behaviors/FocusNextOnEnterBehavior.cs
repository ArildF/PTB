using System.Windows.Input;

namespace Rogue.Ptb.UI.Behaviors
{
	public class FocusNextOnEnterBehavior : TraverseFocusOnKeyBehavior
	{
		public FocusNextOnEnterBehavior()
		{
			Direction = FocusNavigationDirection.Next;
			Key = Key.Enter;
		}
	}
}
