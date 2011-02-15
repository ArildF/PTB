using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Commands
{
	public class OpenTaskBoard : CommandBase<string>
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

		protected override void Execute(string path)
		{
			if (path == null)
			{
				var result = _dialogDisplayer.ShowDialogFor<OpenTaskBoardDialogResult>();
				if (result == null)
				{
					return;
				}
				path = result.Path;
			}
			_sessionFactoryProvider.OpenDatabase(path);
			_bus.Publish(new DatabaseChanged(path));
		}
	}
}
