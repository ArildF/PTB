using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Rogue.Ptb.UI.Behaviors
{
	public class CommandForEventBehavior : Behavior<UIElement>
	{
		private string _event;

		/// <summary>
		/// Identifies the name dependency property.
		/// </summary>
		public static DependencyProperty CommandProperty =
			DependencyProperty.Register("Command", typeof (ICommand), typeof (CommandForEventBehavior), new PropertyMetadata(OnCommandPropertyChanged));

		private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var behavior = (CommandForEventBehavior) d;
			behavior.AttachEventToCommand();
		}

		/// <summary>
		/// 
		/// </summary>
		public ICommand Command
		{
			get { return (ICommand) GetValue(CommandProperty); }
			set { SetValue(CommandProperty, value); }
		}


		
		public string Event
		{
			get { return _event; }
			set
			{
				_event = value;
				AttachEventToCommand();
			}
		}

		protected override void OnAttached()
		{
			AttachEventToCommand();
		}

		private void AttachEventToCommand()
		{
			if (AssociatedObject == null || Event == null || Command == null)
			{
				return;
			}

			EventInfo eventInfo = AssociatedObject.GetType().GetEvent(Event, BindingFlags.Public | BindingFlags.Instance);
			if (eventInfo == null)
			{
				throw new InvalidOperationException(
					string.Format("Cannot find event '{0}' on type '{1}'", Event, AssociatedObject.GetType()));
			}

			var methodInfo = this.GetType().GetMethod("OnEvent", BindingFlags.NonPublic | BindingFlags.Instance);

			var del = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo);
			eventInfo.AddEventHandler(AssociatedObject, del);
		}

		private void OnEvent(object sender, EventArgs e)
		{
			if (Command != null && Command.CanExecute(null))
			{
				Command.Execute(null);

				var handled = e.GetType().GetProperty("Handled");
				if (handled != null && handled.CanWrite)
				{
					handled.SetValue(e, true, null);
				}
			}
		}
	}
}
