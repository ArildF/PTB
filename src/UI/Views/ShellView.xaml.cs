using System.Windows;
using System.Windows.Controls;
using ModernWpf;

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

			MainGrid.Children.Add(toolbarView.Element);
			Grid.SetRow(toolbarView.Element, 0);

			MainGrid.Children.Add(taskBoardView.Element);
			Grid.SetRow(taskBoardView.Element, 1);
			
			ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
			ControlzEx.Theming.ThemeManager.Current.ChangeThemeBaseColor(Application.Current, "Dark");
		}

		public Window Window
		{
			get { return this; }
		}
	}
}
