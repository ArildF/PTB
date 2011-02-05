using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.Commands
{
	public class OpenTaskBoard : NoParameterCommandBase
	{
		private IEventAggregator _bus;
		private ISessionFactoryProvider _sessionFactoryProvider;

		public OpenTaskBoard(IEventAggregator bus, ISessionFactoryProvider sessionFactoryProvider)
		{
			_bus = bus;
			_sessionFactoryProvider = sessionFactoryProvider;
		}

		protected override void Execute()
		{
			_sessionFactoryProvider.OpenDatabase("Board.taskboard");
			_bus.Publish<DatabaseChanged>();
		}
	}
}
