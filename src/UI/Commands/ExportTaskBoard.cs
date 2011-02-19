using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.Commands
{
	public class ExportTaskBoard : NoParameterCommandBase
	{
		private readonly IDialogDisplayer _dialogDisplayer;
		private readonly ITasksExporter _exporter;

		public ExportTaskBoard(IDialogDisplayer dialogDisplayer, ITasksExporter exporter)
		{
			_dialogDisplayer = dialogDisplayer;
			_exporter = exporter;
		}

		protected override void Execute()
		{
			var result = _dialogDisplayer.ShowDialogFor<ExportTaskBoardDialogResult>();
			if (result == null)
			{
				return;
			}

			_exporter.ExportAll(result.Path);

		}
	}
}
