using System.Diagnostics;
using ReactiveUI;
using Rogue.Ptb.Core;
using System.Linq;
using System;

namespace Rogue.Ptb.UI.ViewModels
{
	[DebuggerDisplay("Task: '{Title}'")]
	public class TaskViewModel : ViewModelBase
	{
		private readonly Task _task;
		private bool _isEditing;


		public TaskViewModel(Task task)
		{
			_task = task;
			_isEditing = false;
		}

		public TaskState State
		{
			get { return _task.State; }
			set
			{
				this.RaisePropertyChanging(t => t.State);

				ApplyNewState(value);

				this.RaisePropertyChanged(t => t.State);
				this.RaisePropertyChanged(t => t.ToolTip);
			}
		}

		private void ApplyNewState(TaskState value)
		{
			if (value == _task.State)
			{
				return;
			}

			if (value == TaskState.InProgress)
			{
				_task.Start();
			}
			if (value == TaskState.Complete)
			{
				_task.Complete();
			}
			if (value == TaskState.Abandoned)
			{
				_task.Abandon();
			}

			if (value == TaskState.NotStarted)
			{
				_task.NotStarted();
			}

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

		public bool IsEditing
		{
			get {
				return _isEditing;
			}
			set {
				this.RaiseAndSetIfChanged(tvm => tvm.IsEditing, value);
			}
		}

		public string ToolTip
		{
			get
			{
				Func<DateTime?, string, string> formatter = (date, format) =>
					{
						if (date != null)
						{
							return String.Format("\r\n{0}: {1:g}", format, date);
						}
						return "";
					};

				string str = formatter(_task.CreatedDate, "Created");
				str += formatter(_task.ModifiedDate, "Last modified");
				str += formatter(_task.StartedDate, "Started");
				str += formatter(_task.CompletedDate, "Completed");
				str += formatter(_task.AbandonedDate, "Abandoned");

				str += formatter(_task.StateChangedDate, "State last changed");

				return str.Trim();
			}
		}

		public void BeginEdit()
		{
			IsEditing = true;
		}

		public void EndEdit()
		{
			IsEditing = false;
		}

		public void IsMoreImportantThan(TaskViewModel leastImportant)
		{
			raisePropertyChanging(null);

			_task.IsMoreImportantThan(leastImportant.Task);

			raisePropertyChanged(null);
		}
	}
}
