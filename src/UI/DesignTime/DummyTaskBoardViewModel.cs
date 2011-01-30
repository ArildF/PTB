using System;
using System.Collections.ObjectModel;
using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.DesignTime
{
	public class DummyTaskBoardViewModel
	{
		public DummyTaskBoardViewModel()
		{
			Tasks = new ObservableCollection<DummyTaskViewModel>
				{
					new DummyTaskViewModel(TaskState.InProgress),
					new DummyTaskViewModel(TaskState.Complete),
					new DummyTaskViewModel(TaskState.NotStarted)
				};
		}

		public ObservableCollection<DummyTaskViewModel> Tasks { get; set; }
	}
}
