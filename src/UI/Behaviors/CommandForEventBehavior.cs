using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Rogue.Ptb.UI.Behaviors
{
	public class CommandForEventBehavior : CommandBehavior
	{
		private string _event;


		public string Event
		{
			get { return _event; }
			set
			{
				_event = value;
				ImportantPropertyChanged();
			}
		}

		public int? ClickCount { get; set; }

		public bool Passthrough { get; set; }


		protected override void ImportantPropertyChanged()
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
			if (ClickCount != null && 
			    e is MouseButtonEventArgs mbea && ClickCount > mbea.ClickCount)
			{
				return;
			}
			
			bool handled = InvokeCommandOrAction();

			if (handled)
			{
				var handledProp = e.GetType().GetProperty("Handled");
				if (handledProp != null && handledProp.CanWrite && !Passthrough)
				{
					handledProp.SetValue(e, true, null);
				}
				
			}
		}
	}
}
