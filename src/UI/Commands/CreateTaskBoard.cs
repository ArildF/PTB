using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Commands
{
	public class CreateTaskBoard : NoParameterCommandBase<CreateTaskBoard>
	{
		private readonly ISessionFactoryProvider _sessionFactoryProvider;
		private readonly IEventAggregator _bus;
		private readonly IDialogDisplayer _dialogDisplayer;

		public CreateTaskBoard(ISessionFactoryProvider sessionFactoryProvider, IEventAggregator bus,
			IDialogDisplayer dialogDisplayer) : base(bus)
		{
			_sessionFactoryProvider = sessionFactoryProvider;
			_bus = bus;
			_dialogDisplayer = dialogDisplayer;
		}


		protected override void Execute()
		{
			var result = _dialogDisplayer.ShowDialogFor<CreateTaskBoardDialogResult>();
			if (result == null)
			{
				return;
			}

			_sessionFactoryProvider.CreateNewDatabase(result.Path);
			_bus.Publish(new DatabaseChanged(result.Path));
		}
	}
}
