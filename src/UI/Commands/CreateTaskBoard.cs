using ReactiveUI;
using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.Commands
{
	public class CreateTaskBoard : NoParameterCommandBase
	{
		private readonly ISessionFactoryProvider _sessionFactoryProvider;
		private readonly IEventAggregator _bus;

		public CreateTaskBoard(ISessionFactoryProvider sessionFactoryProvider, IEventAggregator bus)
		{
			_sessionFactoryProvider = sessionFactoryProvider;
			_bus = bus;
		}


		protected override void Execute()
		{
			_sessionFactoryProvider.CreateNewDatabase("Board.taskboard");
			_bus.Publish<DatabaseChanged>();
		}
	}
}
