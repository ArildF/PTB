using System;
using System.Windows.Input;

namespace Rogue.Ptb.UI.Views
{
	/// <summary>
	/// Interaction logic for DialogHost.xaml
	/// </summary>
	public partial class DialogHost
	{
		private readonly Dialog _content;

		public DialogHost()
		{
			InitializeComponent();
		}

		public DialogHost(IShellView shellView, Dialog content) : this()
		{
			_content = content;
			Width = shellView.Window.Width;
			Height = shellView.Window.Height;

			Left = shellView.Window.Left;
			Top = shellView.Window.Top;

			_contentHost.Content = content;

			DataContext = this;

			content.CloseResult.Subscribe(result => DialogResult = result);
		}

		public ICommand OkCommand
		{
			get { return _content.OkCommand; }
		}

	}
}
