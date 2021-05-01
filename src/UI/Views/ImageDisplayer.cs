using System.Collections.Generic;
using System.Windows.Controls;

namespace Rogue.Ptb.UI.Views
{
	public class ImageDisplayer
	{
		private readonly Dictionary<Image, ImageWindow> _windows = new();

		public void Show(Image image)
		{
			if (!_windows.TryGetValue(image, out var window) || !window.IsLoaded)
			{
				window = new ImageWindow(image);
				_windows[image] = window;
			}
			window.Show();
			window.Activate();
		}
	}
}