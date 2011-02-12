using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Win32;
using ReactiveUI.Xaml;

namespace Rogue.Ptb.UI.Views
{
	public class OpenTaskBoardDialog : TaskBoardPathDialogBase
	{
		public OpenTaskBoardDialog()
		{
			var canExecute = Observable.FromEvent<TextChangedEventArgs>(_pathTextBox, "TextChanged")
				.Select(ea => _pathTextBox.Text)
				.Select(path => !String.IsNullOrEmpty(path) &&
					File.Exists(path) &&
					path.EndsWith(".taskboard", StringComparison.InvariantCultureIgnoreCase));

			var command = new ReactiveCommand(canExecute);
			command.Subscribe(_ => ReturnValue = new OpenTaskBoardDialogResult(_pathTextBox.Text));

			OkCommand = command;

			Title = "Open taskboard";
		}

		protected override string BrowseForPath()
		{
			var openFileDialog = new OpenFileDialog
				{
					FileName = "My taskboard",
					DefaultExt = ".taskboard",
					Filter = "Taskboard files (.taskboard)|*.taskboard"
				};

			var result = openFileDialog.ShowDialog();
			if (result.GetValueOrDefault())
			{
				return openFileDialog.FileName;
			}
			return "";
		}
	}
}
