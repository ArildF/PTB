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

		private string _action;

		private static void OnCommandPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var behavior = (CommandForEventBehavior) d;
			behavior.AttachToEvent();
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
				AttachToEvent();
			}
		}


		public string Action
		{
			get { return _action;  }
			set
			{
				_action = value;
	
				AttachToEvent();
			}

		}

		protected override void OnAttached()
		{
			AttachToEvent();
		}

		private void AttachToEvent()
		{
			if (AssociatedObject == null || Event == null || (Command == null && Action == null))
			{
				return;
			}

			EventInfo eventInfo = AssociatedObject.GetType().GetEvent(Event, BindingFlags.Public | BindingFlags.Instance);
			if (eventInfo == null)
			{
				throw new InvalidOperationException(
					string.Format("Cannot find event '{0}' on type '{1}'", Event, AssociatedObject.GetType()));
			}

			var methodInfo = GetType().GetMethod("OnEvent", BindingFlags.NonPublic | BindingFlags.Instance);

			if (eventInfo.EventHandlerType != null)
			{
				var del = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, methodInfo);
				eventInfo.AddEventHandler(AssociatedObject, del);
			}
			else
			{
				throw new InvalidOperationException("This shouldn't happen. Really. Believe me.");
			}
		}

		private void OnEvent(object sender, EventArgs e)
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
				var method = dataContext.GetType().GetMethod(_action, BindingFlags.Public | BindingFlags.Instance);
				method.Invoke(dataContext, null);
				handled = true;
			}

			if (handled)
			{
				var handledProp = e.GetType().GetProperty("Handled");
				if (handledProp != null && handledProp.CanWrite)
				{
					handledProp.SetValue(e, true, null);
				}
				
			}
		}
	}
}
