using System;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Win32;
using ReactiveUI.Xaml;

namespace Rogue.Ptb.UI.Views
{
	public class CreateTaskBoardDialog : TaskBoardPathDialogBase
	{
		public CreateTaskBoardDialog()
		{
			var canExecute = Observable.FromEvent<TextChangedEventArgs>(_pathTextBox, "TextChanged")
				.Select(ea => _pathTextBox.Text)
				.Select(path => !String.IsNullOrEmpty(path) && 
					Directory.Exists(Path.GetDirectoryName(path)) &&
					path.EndsWith(".taskboard", StringComparison.InvariantCultureIgnoreCase));

			var command = new ReactiveCommand(canExecute);
			command.Subscribe(OnExecute);

			OkCommand = command;
			Title = "Create taskboard";

		}

		private void OnExecute(object o)
		{
			ReturnValue = new CreateTaskBoardDialogResult(_pathTextBox.Text);
		}

		protected override string BrowseForPath()
		{
			var saveFileDialog = new SaveFileDialog
				{
					FileName = "My taskboard",
					DefaultExt = ".taskboard",
					Filter = "Taskboard files (.taskboard)|*.taskboard"
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
