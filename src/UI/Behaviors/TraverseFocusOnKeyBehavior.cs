using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Rogue.Ptb.UI.Behaviors
{
	public class TraverseFocusOnKeyBehavior : Behavior<Control>
	{
		public Key Key { get; set; }

		public FocusNavigationDirection Direction { get; set; }

		protected override void OnAttached()
		{
			AssociatedObject.KeyDown += AssociatedObjectOnKeyDown;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.KeyDown -= AssociatedObjectOnKeyDown;
		}

		private void AssociatedObjectOnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key != Key)
			{
				return;
			}

			AssociatedObject.MoveFocus(new TraversalRequest(Direction));
		}
	}
}
