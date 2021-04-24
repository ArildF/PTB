using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Commands
{
	public class ExportTaskBoard : NoParameterCommandBase<ExportTaskBoard>
	{
		private readonly IDialogDisplayer _dialogDisplayer;
		private readonly ITasksExporter _exporter;

		public ExportTaskBoard(IDialogDisplayer dialogDisplayer, ITasksExporter exporter,
			IEventAggregator bus) : base(bus)
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
