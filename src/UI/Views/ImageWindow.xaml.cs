using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Rogue.Ptb.UI.Views
{
	public partial class ImageWindow
	{

		public ImageWindow(Image image)
		{
			InitializeComponent();
			Image.Source = image.Source;
		}

		private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (Image.Source is BitmapSource bs)
			{
				Clipboard.SetImage(bs);
			}
		}

		private void CommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = Image.Source is BitmapSource;
		}
	}
}