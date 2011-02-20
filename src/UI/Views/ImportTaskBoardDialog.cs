using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Win32;
using ReactiveUI.Xaml;

namespace Rogue.Ptb.UI.Views
{
	public class ImportTaskBoardDialog : TaskBoardPathDialogBase
	{
		public ImportTaskBoardDialog()
		{
			var canExecute = from ea in Observable.FromEvent<TextChangedEventArgs>(_pathTextBox, "TextChanged")
			                 let path = _pathTextBox.Text
			                 select File.Exists(path);

			var command = new ReactiveCommand(canExecute);
			command.Subscribe(_ => ReturnValue = new ImportTaskBoardDialogResult(_pathTextBox.Text));

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
