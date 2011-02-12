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
	/// Interaction logic for TaskBoardPathDialogBase.xaml
	/// </summary>
	public partial class TaskBoardPathDialogBase 
	{
		public TaskBoardPathDialogBase()
		{
			InitializeComponent();
			_pathTextBox.Text = "";

			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			DataContext = this;
		}

		public string Title { get; protected set; }


		private void ButtonOnClick(object sender, RoutedEventArgs e)
		{
			_pathTextBox.Text = BrowseForPath();
		}

		protected virtual string BrowseForPath()
		{
			return "";
		}
	}
}
