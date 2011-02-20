using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Commands
{
	public class ImportTaskBoard : NoParameterCommandBase
	{
		private readonly ITasksImporter _importer;
		private readonly IEventAggregator _eventAggregator;
		private readonly IDialogDisplayer _dialogDisplayer;

		public ImportTaskBoard(ITasksImporter importer, IEventAggregator eventAggregator, IDialogDisplayer dialogDisplayer)
		{
			_importer = importer;
			_eventAggregator = eventAggregator;
			_dialogDisplayer = dialogDisplayer;
		}

		protected override void Execute()
		{
			var result = _dialogDisplayer.ShowDialogFor<ImportTaskBoardDialogResult>();
			if (result == null)
			{
				return;
			}

			_importer.ImportAll(result.Path);

			_eventAggregator.Publish(new ReloadAllTasks());
		}
	}
}
