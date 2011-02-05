using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReactiveUI;
using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.ViewModels
{
	public class TaskViewModel : ReactiveObject
	{
		private readonly Task _task;

		private string _title;
		private TaskState _state;

		public TaskViewModel(Task task)
		{
			_task = task;
			_title = task.Title;
			_state = task.State;
		}

		public TaskState State
		{
			get { return _state; }
			set { this.RaiseAndSetIfChanged(t => t.State, value); }
		}

		public string Title
		{
			get { return _title; }
			set { this.RaiseAndSetIfChanged(t => t.Title, value); }
		}

		public Task Task
		{
			get {
				return _task;
			}
		}
	}
}
