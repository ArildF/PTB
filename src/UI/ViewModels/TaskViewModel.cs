using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using Rogue.Ptb.Core;
using System;
using System.Reactive;

namespace Rogue.Ptb.UI.ViewModels
{
	[DebuggerDisplay("Task: '{Title}'")]
	public class TaskViewModel : ViewModelBase
	{
		private readonly Task _task;
		private readonly Func<Task, TaskViewModel> _viewModelMapper;
		private bool _isEditing;
		private bool _isSelected;
		private bool _isVisible;


		public TaskViewModel(Task task, Func<Task, TaskViewModel> viewModelMapper)
		{
			_task = task;
			_viewModelMapper = viewModelMapper;
			_isEditing = false;
			_isVisible = true;


			var cmd = ReactiveCommand.Create<Unit>(_ => ToggleCollapseHierarchy());
			cmd.Subscribe();

			ToggleCollapseHierarchyCommand = cmd;
		}

		public TaskState State
		{
			get { return _task.State; }
			set
			{
				if (value == _task.State)
				{
					return;
				}

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
			get => _task.Title;
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
				this.RaiseAndSetIfChanged(ref _isEditing, value);
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

		public bool IsSelected
		{
			get => _isSelected;
			set => this.RaiseAndSetIfChanged(ref _isSelected, value);
		}

		public int IndentLevel => _task.AncestorCount();

		public bool IsVisible
		{
			get => _isVisible;
			private set => this.RaiseAndSetIfChanged(ref _isVisible, value);
		}

		public ICommand ToggleCollapseHierarchyCommand { get; private set; }

		public bool CanCollapse
		{
			get { return ChildVMs().Any(vm => vm.IsVisible); }
		}

		public bool CanExpand
		{
			get { return ChildVMs().Any(vm => !vm.IsVisible); }
		}

		public bool Collapsable
		{
			get { return _task.SubTasks.Any(); }
		}

		public void BeginEdit()
		{
			Select();
			IsEditing = true;
		}

		public void EndEdit()
		{
			IsEditing = false;
		}

		public void IsMoreImportantThan(TaskViewModel leastImportant)
		{
			this.RaisePropertyChanging();

			_task.IsMoreImportantThan(leastImportant.Task);

			this.RaisePropertyChanged();
		}

		public TaskViewModel CreateSubTask()
		{
			return new(_task.CreateSubTask(), _viewModelMapper);
		}

		public void Select()
		{
			IsSelected = true;
		}

		public bool CanMakeMoreImportantThan(TaskViewModel leastImportant)
		{
			return _task.CanBeMoreImportantThan(leastImportant.Task);
		}

		public void Deselect()
		{
			IsSelected = false;
		}

		public void NotifyStateChanged()
		{
			this.RaisePropertyChanged(t => t.State);
		}

		public IEnumerable<TaskViewModel> ChildVMs()
		{
			return _task.SubTasks.Select(_viewModelMapper);
		}

		public void Hide()
		{
			IsVisible = false;
		}

		public void Show()
		{
			IsVisible = true;
		}

		private void ToggleCollapseHierarchy()
		{
			this.RaisePropertyChanging(vm => vm.CanCollapse);
			this.RaisePropertyChanging(vm => vm.CanExpand);

			bool collapse = CanCollapse;
			foreach (var taskViewModel in ChildVMs())
			{
				taskViewModel.ToggleCollapseHierarchy();

				if (collapse)
				{
					taskViewModel.Hide();
				}
				else
				{
					taskViewModel.Show();
				}

			}

			this.RaisePropertyChanged(vm => vm.CanCollapse);
			this.RaisePropertyChanged(vm => vm.CanExpand);
		}
	}
}
