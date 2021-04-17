using System.Windows;

namespace Rogue.Ptb.UI.Services
{
	public interface IMessageBoxService
	{
		MessageBoxResult ShowYesNoDialog(string text);
	}

	public class MessageBoxService : IMessageBoxService
	{
		private readonly IShellView _view;

		public MessageBoxService(IShellView view)
		{
			this._view = view;
		}

		public MessageBoxResult ShowYesNoDialog(string text)
		{
			return MessageBox.Show(text, "", MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.No);
		}
	}
}