using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ReactiveUI;

namespace Rogue.Ptb.UI.Infrastructure
{
	// inspired by http://social.msdn.microsoft.com/forums/en-US/wpf/thread/5909dbcc-9a9f-4260-bc36-de4aa9bbd383/
	public class SortableReactiveCollection<T> : ReactiveCollection<T>
	{
		public void Sort(IComparer<T> comparer)
		{
			var orderedList = this.ToList(); 
			orderedList.Sort(comparer);

			foreach (var item in orderedList)
			{
				Move(IndexOf(item), orderedList.IndexOf(item));
			}

		}
	}
}
