using System.Windows;
using System.Windows.Controls;

namespace Rogue.Ptb.UI.Views
{
	/// <summary>
	/// Interaction logic for ShellView.xaml
	/// </summary>
	public partial class ShellView : IShellView
	{
		public ShellView()
		{
			InitializeComponent();
		}

		public ShellView(IShellViewModel vm, IToolbarView toolbarView, ITaskBoardView taskBoardView) : this()
		{
			DataContext = vm;

			mainGrid.Children.Add(toolbarView.Element);
			Grid.SetRow(toolbarView.Element, 0);

			mainGrid.Children.Add(taskBoardView.Element);
			Grid.SetRow(taskBoardView.Element, 1);
		}

		public Window Window
		{
			get { return this; }
		}
	}
}
