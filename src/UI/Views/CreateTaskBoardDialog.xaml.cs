using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using ReactiveUI.Xaml;

namespace Rogue.Ptb.UI.Views
{
	/// <summary>
	/// Interaction logic for CreateTaskBoardDialog.xaml
	/// </summary>
	public partial class CreateTaskBoardDialog 
	{
		public CreateTaskBoardDialog()
		{
			InitializeComponent();

			var canExecute = Observable.FromEvent<TextChangedEventArgs>(_pathTextBox, "TextChanged")
				.Select(ea => _pathTextBox.Text)
				.Select(path => !String.IsNullOrEmpty(path) && 
					Directory.Exists(Path.GetDirectoryName(path)) &&
					path.EndsWith(".taskboard", StringComparison.InvariantCultureIgnoreCase));

			var command = new ReactiveCommand(canExecute);
			command.Subscribe(OnExecute);

			OkCommand = command;

			_pathTextBox.Text = "";
		}

		private void OnExecute(object o)
		{
			ReturnValue = new CreateTaskBoardDialogResult(_pathTextBox.Text);
		}

		private void ButtonOnClick(object sender, RoutedEventArgs e)
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
				_pathTextBox.Text = saveFileDialog.FileName;
			}

		}
	}
}
