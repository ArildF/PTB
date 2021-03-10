using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Rogue.Ptb.UI.Behaviors
{
	public abstract class CommandBehavior : Behavior<UIElement>
	{
		/// <summary>
		/// Identifies the name dependency property.
		/// </summary>
		public static DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof (ICommand), typeof (CommandForEventBehavior), new PropertyMetadata(OnCommandPropertyChanged));

		private string _action;

		/// <summary>
		/// 
		/// </summary>
		public ICommand Command
		{
			get { return (ICommand) GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}

		public string Action
		{
			get { return _action;  }
			set
			{
				_action = value;
	
				ImportantPropertyChanged();
			}

		}

		protected abstract void ImportantPropertyChanged();

		private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var behavior = (CommandForEventBehavior) d;
			behavior.ImportantPropertyChanged();
		}

		protected override void OnAttached()
		{
			ImportantPropertyChanged();
		}

		protected bool InvokeCommandOrAction()
		{
			bool handled = false;
			if (Command != null && Command.CanExecute(null))
			{
				Command.Execute(null);
				handled = true;

			}

			object dataContext;
			if (Action != null && (dataContext = AssociatedObject.GetValue(FrameworkElement.DataContextProperty)) != null)
			{
				var method = dataContext.GetType().GetMethod(Action, BindingFlags.Public | BindingFlags.Instance);
				method.Invoke(dataContext, null);
				handled = true;
			}
			return handled;
		}
	}
}
