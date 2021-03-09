using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Controls;
using Microsoft.Win32;
using ReactiveUI;

namespace Rogue.Ptb.UI.Views
{
	public class OpenTaskBoardDialog : TaskBoardPathDialogBase
	{
		public OpenTaskBoardDialog()
		{
			var canExecute = Observable.FromEventPattern<TextChangedEventArgs>(_pathTextBox, "TextChanged")
				.Select(_ => _pathTextBox.Text)
				.Select(path => !String.IsNullOrEmpty(path) &&
					File.Exists(path) &&
					path.EndsWith(".taskboard", StringComparison.InvariantCultureIgnoreCase));

			var command = ReactiveCommand.Create<Unit>(_ => ReturnValue = new OpenTaskBoardDialogResult(_pathTextBox.Text), canExecute);

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
