using System.Windows.Input;

namespace Rogue.Ptb.UI.Behaviors
{
	public class KeyCommandBehavior : CommandBehavior
	{
		private Key _key;
		public Key Key
		{
			get { return _key; }
			set
			{
				_key = value;
				ImportantPropertyChanged();
			}
		}

		public KeyCommandBehavior()
		{
			Key = Key.None;
		}
		protected override void ImportantPropertyChanged()
		{
			if (AssociatedObject == null || (Action == null && Command == null) || Key == Key.None)
			{
				return;
			}
			
			AssociatedObject.KeyDown += AssociatedObjectOnKeyDown;
		}

		private void AssociatedObjectOnKeyDown(object sender, KeyEventArgs keyEventArgs)
		{
			InvokeCommandOrAction();
		}
	}
}
