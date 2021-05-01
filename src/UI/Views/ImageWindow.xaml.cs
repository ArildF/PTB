using System.Windows.Controls;

namespace Rogue.Ptb.UI.Views
{
	public partial class ImageWindow
	{
		public ImageWindow(Image image)
		{
			InitializeComponent();
			Image.Source = image.Source;
		}
	}
}