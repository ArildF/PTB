using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Commands
{
	public class EventCommand<T> : NoParameterCommandBase
	{
		private readonly IEventAggregator _bus;

		public EventCommand(IEventAggregator bus)
		{
			_bus = bus;
		}

		protected override void Execute()
		{
			_bus.Publish<T>();
		}
	}
}
