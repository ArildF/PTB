using ReactiveUI;

namespace Rogue.Ptb.UI.ViewModels
{
	public class ViewModelBase : ReactiveObject, IReactivePropertyChangingObject
	{
		public void OnPropertyChanging(string propertyName)
		{
			raisePropertyChanging(propertyName);
		}

		public void OnPropertyChanged(string propertyName)
		{
			raisePropertyChanged(propertyName);
		}
	}
}
