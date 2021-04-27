using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using Rogue.Ptb.Core;
using System;
using System.Reactive;
using Rogue.Ptb.UI.Views;

namespace Rogue.Ptb.UI.ViewModels
{
	[DebuggerDisplay("Task: '{Title}'")]
	public class TaskViewModel : ViewModelBase
	{
		private readonly Task _task;
		private readonly Func<Task, TaskViewModel> _viewModelMapper;
		private readonly NoteDisplayer _displayer;
		private bool _isEditing;
		private bool _isSelected;
		private bool _isVisible;


		public TaskViewModel(Task task, Func<Task, TaskViewModel> viewModelMapper, NoteDisplayer displayer)
		{
			_task = task;
			_viewModelMapper = viewModelMapper;
			_displayer = displayer;
			_isEditing = false;
			_isVisible = true;


			var cmd = ReactiveCommand.Create<Unit>(_ => ToggleCollapseHierarchy());
			cmd.Subscribe();

			ToggleCollapseHierarchyCommand = cmd;

			ShowNotesCommand = ReactiveCommand.Create<Unit>(_ => displayer.Display(_task));
		}
		
		public ICommand ShowNotesCommand { get; }

		public TaskState State
		{
			get => _task.State;
			set
			{
				if (value == _task.State)
				{
					return;
				}

				this.RaisePropertyChanging();

				ApplyNewState(value);

				this.RaisePropertyChanged();
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
				Progress = _task.Progress ?? 0;
			}
			if (value == TaskState.Abandoned)
			{
				_task.Abandon();
			}

			if (value == TaskState.NotStarted)
			{
				_task.NotStarted();
				Progress = _task.Progress ?? 0;
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
			get => _isEditing;
			set => this.RaiseAndSetIfChanged(ref _isEditing, value);
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

		public bool IsProgressEnabled => _task.IsLeaf;

		private IEnumerable<TaskViewModel> NonAbandonedChildVMs => ChildVMs().Where(cvm => cvm.State != TaskState.Abandoned);

		public double Progress
		{
			get => _task.IsLeaf
				? _task.Progress ?? 0
				: NonAbandonedChildVMs.Any()
					? NonAbandonedChildVMs.Sum(vm => vm.Progress) / NonAbandonedChildVMs.Count()
					: 100;
			set
			{
				_task.Progress = value;
				NotifyProgressChanged();
				if (_task.Parent != null)
				{
					var parentVM = _viewModelMapper(_task.Parent);
					parentVM.NotifyProgressChanged();
				}
			}
		}

		public void NotifyProgressChanged()
		{
			var parentVM = _viewModelMapper(_task.Parent);
			parentVM?.NotifyProgressChanged();
			this.RaisePropertyChanged(vm => vm.Progress);
			
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
			return new(_task.CreateSubTask(), _viewModelMapper, _displayer);
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
			IsEditing = false;
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

		public void NewChildAdded()
		{
			this.RaisePropertyChanged(vm => vm.Progress);
			this.RaisePropertyChanged(vm => vm.Collapsable);
			this.RaisePropertyChanged(vm => vm.CanCollapse);
			this.RaisePropertyChanged(vm => vm.CanExpand);
		}
	}
}
