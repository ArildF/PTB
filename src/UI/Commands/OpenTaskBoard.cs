using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Commands
{
	public class OpenTaskBoard : NoParameterCommandBase
	{
		private readonly IEventAggregator _bus;
		private readonly ISessionFactoryProvider _sessionFactoryProvider;
		private readonly IDialogDisplayer _dialogDisplayer;

		public OpenTaskBoard(IEventAggregator bus, ISessionFactoryProvider sessionFactoryProvider, 
			IDialogDisplayer dialogDisplayer)
		{
			_bus = bus;
			_sessionFactoryProvider = sessionFactoryProvider;
			_dialogDisplayer = dialogDisplayer;
		}

		protected override void Execute()
		{
			var result = _dialogDisplayer.ShowDialogFor<OpenTaskBoardDialogResult>();
			if (result == null)
			{
				return;
			}
			_sessionFactoryProvider.OpenDatabase(result.Path);
			_bus.Publish<DatabaseChanged>();
		}
	}
}
