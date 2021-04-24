using System.Diagnostics;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Commands
{
	[DebuggerDisplay("EventCommand<{typeof(T)}>")]
	public class EventCommand<T> : NoParameterCommandBase<EventCommand<T>>
	{
		private readonly IEventAggregator _bus;

		public EventCommand(IEventAggregator bus) : base(bus)
		{
			_bus = bus;
		}

		protected override void Execute()
		{
			_bus.Publish<T>();
		}
	}
}
