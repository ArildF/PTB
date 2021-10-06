using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Rogue.Ptb.UI.Views
{
	public partial class ImageWindow
	{
		private readonly Image _image;

		public ImageWindow(Image image)
		{
			_image = image;
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
	}
}