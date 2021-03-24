using System;
using System.Linq;
using ReactiveUI;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.ViewModels
{
	public class ViewModelBase : ReactiveObject//, IReactivePropertyChangingObject
	{
		// void IReactivePropertyChangingObject.OnPropertyChanging(string propertyName)
		// {
		// 	((IReactivePropertyChangingObject)base).OnPropertyChanging(propertyName);
		// }
		//
		// void IReactivePropertyChangingObject.OnPropertyChanged(string propertyName)
		// {
		// 	((IReactivePropertyChangingObject)this).OnPropertyChanged(propertyName);
		// }
	}
}
