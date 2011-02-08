using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Commands
{
	public class OpenTaskBoard : NoParameterCommandBase
	{
		private readonly IEventAggregator _bus;
		private readonly ISessionFactoryProvider _sessionFactoryProvider;

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
