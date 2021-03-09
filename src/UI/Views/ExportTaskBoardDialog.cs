using System;
using System.IO;
using System.Reactive.Linq;
using System.Windows.Controls;
using Microsoft.Win32;
using ReactiveUI;

namespace Rogue.Ptb.UI.Views
{
	public class ExportTaskBoardDialog : TaskBoardPathDialogBase
	{
		public ExportTaskBoardDialog()
		{
			var canExecute = from ea in Observable.FromEventPattern(_pathTextBox, "TextChanged")
			                 let path = _pathTextBox.Text
			                 let directory = Path.GetDirectoryName(path)
			                 select Directory.Exists(directory);

			var command = ReactiveCommand.Create(() => ReturnValue = new ExportTaskBoardDialogResult(_pathTextBox.Text), canExecute);
			OkCommand = command;

			Title = "Export taskboard";
		}

		protected override string BrowseForPath()
		{
			var saveFileDialog = new SaveFileDialog
			{
				FileName = "My taskboard",
				DefaultExt = ".taskboard",
				Filter = "XML files (*.xml)|*.xml"
			};

			var result = saveFileDialog.ShowDialog();
			if (result.GetValueOrDefault())
			{
				return saveFileDialog.FileName;
			}
			return "";
		}
	}
}
