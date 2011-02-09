using System;
using System.Linq;
using ReactiveUI;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.ViewModels
{
	public class ViewModelBase : ReactiveObject, IReactivePropertyChangingObject
	{
		void IReactivePropertyChangingObject.OnPropertyChanging(string propertyName)
		{
			raisePropertyChanging(propertyName);
		}

		void IReactivePropertyChangingObject.OnPropertyChanged(string propertyName)
		{
			raisePropertyChanged(propertyName);
		}
	}
}
