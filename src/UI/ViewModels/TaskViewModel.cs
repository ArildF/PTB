using ReactiveUI;
using Rogue.Ptb.Core;
using System.Linq;
using System;

namespace Rogue.Ptb.UI.ViewModels
{
	public class TaskViewModel : ViewModelBase
	{
		private readonly Task _task;


		public TaskViewModel(Task task)
		{
			_task = task;
		}

		public TaskState State
		{
			get { return _task.State; }
			set { this.SetAndRaisePropertyChanged(t => t.State, value, val => _task.State = val); }
		}

		public string Title
		{
			get { return _task.Title; }
			set { this.SetAndRaisePropertyChanged(t => t.Title, value, val  => _task.Title = val); }
		}

		public Task Task
		{
			get {
				return _task;
			}
		}
	}
}
