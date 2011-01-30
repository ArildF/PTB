using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Rogue.Ptb.Core;
using Rogue.Ptb.UI.DesignTime;

namespace Rogue.Ptb.UI.ViewModels
{
	public class TaskBoardViewModel : ITaskBoardViewModel
	{
		public TaskBoardViewModel()
		{
			
			Tasks = new ObservableCollection<DummyTaskViewModel>
				{
					new DummyTaskViewModel(TaskState.InProgress),
					new DummyTaskViewModel(TaskState.Complete),
					new DummyTaskViewModel(TaskState.NotStarted)
				};
		}

		public ObservableCollection<DummyTaskViewModel> Tasks { get; private set; }
	}
}
