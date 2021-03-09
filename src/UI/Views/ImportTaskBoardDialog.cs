using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Controls;
using Microsoft.Win32;
using ReactiveUI;

namespace Rogue.Ptb.UI.Views
{
	public class ImportTaskBoardDialog : TaskBoardPathDialogBase
	{
		public ImportTaskBoardDialog()
		{
			var canExecute = from ea in Observable.FromEventPattern<TextChangedEventArgs>(_pathTextBox, "TextChanged")
			                 let path = _pathTextBox.Text
			                 select File.Exists(path);

			var command = ReactiveCommand.Create<Unit>(_ => ReturnValue = new ImportTaskBoardDialogResult(_pathTextBox.Text),
				canExecute);

			OkCommand = command;

			Title = "Import tasks";
		}

		protected override string BrowseForPath()
		{
			var ofd = new OpenFileDialog
				{
					Filter = "XML Files (*.xml)|*.xml"
				};

			var result = ofd.ShowDialog();
			if (result.GetValueOrDefault())
			{
				return ofd.FileName;
			}
			return "";
		}
	}
}
