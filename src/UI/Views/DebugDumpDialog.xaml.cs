namespace Rogue.Ptb.UI.Views
{
	/// <summary>
	/// Interaction logic for DebugDumpDialog.xaml
	/// </summary>
	public partial class DebugDumpDialog
	{
		public DebugDumpDialog()
		{
			InitializeComponent();
		}

		public DebugDumpDialog(string text) : this()
		{
			_textBox.Text = text;
		}

		private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
