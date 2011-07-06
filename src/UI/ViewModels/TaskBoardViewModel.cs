using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Xaml;
using Rogue.Ptb.Core;
using NHibernate.Linq;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Behaviors;
using Rogue.Ptb.UI.Commands;
using Rogue.Ptb.UI.Infrastructure;

namespace Rogue.Ptb.UI.ViewModels
{
	public class TaskBoardViewModel : ViewModelBase, ITaskBoardViewModel, ICommandResolver
	{
		private readonly IRepositoryProvider _repositoryProvider;
		private IRepository<Task> _repository;
		private readonly IEventAggregator _bus;
		private readonly ICommandResolver _commandResolver;
		private List<Task> _tasks;
		private TaskViewModel _selectedTask;

		public TaskBoardViewModel(IRepositoryProvider repositoryProvider, IEventAggregator bus, ICommandResolver commandResolver)
		{
			_repositoryProvider = repositoryProvider;
			_bus = bus;
			_commandResolver = commandResolver;

			Tasks = new SortableReactiveCollection<TaskViewModel>();

			Tasks.ItemChanged
				.Throttle(TimeSpan.FromSeconds(5))
				.Select(c => c.Sender)
				.Where(c =>  _repository != null)
				.Where(task => !task.IsEditing)
				.SubscribeOn(RxApp.DeferredScheduler)
				.Subscribe(_ => OnSaveAllTasks(null));

			Tasks.ItemChanged
				.Where(c => c.PropertyName == "IsSelected")
				.SubscribeOn(RxApp.DeferredScheduler)
				.Select(c => (TaskViewModel)c.Sender)
				.Subscribe(IsSelectedChanged);


			Tasks.ChangeTrackingEnabled = true;

			_bus.ListenOnScheduler<DatabaseChanged>(OnDatabaseChanged);
			_bus.ListenOnScheduler<CreateNewTask>(OnCreateNewTask);
			_bus.ListenOnScheduler<CreateNewSubTask>(OnCreateNewSubTask);
			_bus.ListenOnScheduler<SaveAllTasks>(OnSaveAllTasks);
			_bus.ListenOnScheduler<ReloadAllTasks>(evt => Reload());
			_bus.ListenOnScheduler<ReSort>(evt => Reorder());

			DragCommand = new ReactiveCommand();


			DragCommand.OfType<DragCommandArgs>().Subscribe(OnNext);

		}


		private void IsSelectedChanged(TaskViewModel viewModel)
		{
			if (!viewModel.IsSelected)
			{
				return;
			}
			if (viewModel == _selectedTask)
			{
				return;
			}
			if (_selectedTask != null)
			{
				_selectedTask.IsSelected = false;
			}

			_selectedTask = viewModel;
			this.RaisePropertyChanged(vm => vm.SelectedTask);
		}

		private void OnNext(DragCommandArgs dragCommandArgs)
		{
			var target = (TaskViewModel) dragCommandArgs.DragTarget;
			var dragged = (TaskViewModel) dragCommandArgs.Dragged;

			int indexTarget = Tasks.IndexOf(target);
			int indexDragged = Tasks.IndexOf(dragged);

			if (indexTarget < 0 || indexDragged < 0)
			{
				return;
			}

			var mostImportant = indexTarget < indexDragged ? dragged : target;
			var leastImportant = mostImportant == target ? dragged : target;

			if (mostImportant.CanMakeMoreImportantThan(leastImportant))
			{
				mostImportant.IsMoreImportantThan(leastImportant);
				Reorder();
			}
		}


		public ReactiveCommand DragCommand { get; private set; }

		public SortableReactiveCollection<TaskViewModel> Tasks { get; private set; }

		public TaskViewModel SelectedTask
		{
			get {
				return _selectedTask;
			}
			set {
				this.RaiseAndSetIfChanged(vm => vm.SelectedTask, value);
				if (value != null && !value.IsSelected)
				{
					value.IsSelected = true;
				}
			}
		}

		private void OnSaveAllTasks(SaveAllTasks ignored)
		{
			var tasks = Tasks.Select(t => t.Task);
			_repository.SaveAll(tasks);
		}

		private void OnDatabaseChanged(DatabaseChanged databaseChanged)
		{
			_repository = NewRepository();

			Reload();
		}

		private void Reload()
		{
			_tasks = _repository.FindAll().ToList();
			Reorder();
		}

		private void Reorder()
		{
			Tasks.Clear();


			_tasks.InPlaceSort();

			_tasks.Select(t => new TaskViewModel(t)).ForEach(Tasks.Add);
		}

		private void OnCreateNewTask(CreateNewTask ignored)
		{
			var task = new Task();

			_repository.InsertNew(task);

			var taskViewModel = new TaskViewModel(task);
			Tasks.Insert(0, taskViewModel);
			_tasks.Add(task);

			if (SelectedTask != null)
			{
				SelectedTask.Deselect();
			}

			SelectedTask = taskViewModel;
			taskViewModel.BeginEdit();
		}

		private void OnCreateNewSubTask(CreateNewSubTask obj)
		{
			if (SelectedTask == null)
			{
				return;
			}

			var newTaskViewModel = SelectedTask.CreateSubTask();
			var indexToInsert = Tasks.IndexOf(SelectedTask) + 1;
			Tasks.Insert(indexToInsert, newTaskViewModel);
			_tasks.Add(newTaskViewModel.Task);

			SelectedTask = newTaskViewModel;

			newTaskViewModel.BeginEdit();
		}

		private IRepository<Task> NewRepository()
		{
			if (_repository != null)
			{
				_repository.Dispose();
			}
			return _repositoryProvider.Open<Task>();
		}

		public ICommand Resolve(CommandName commandName)
		{
			return _commandResolver.Resolve(commandName);
		}

		public void Deselect()
		{
			if (Tasks == null)
			{
				return;
			}
			Tasks.Where(t => t.IsSelected).ForEach(t => t.Deselect());
		}
	}
}
