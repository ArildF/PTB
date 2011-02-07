using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ReactiveUI;
using Rogue.Ptb.Core;
using NHibernate.Linq;

namespace Rogue.Ptb.UI.ViewModels
{
	public class TaskBoardViewModel : ITaskBoardViewModel
	{
		private readonly IRepositoryProvider _repositoryProvider;
		private readonly IEventAggregator _bus;
		private IRepository<Task> _repository;

		public TaskBoardViewModel(IRepositoryProvider repositoryProvider, IEventAggregator bus)
		{
			_repositoryProvider = repositoryProvider;
			_bus = bus;

			Tasks = new ReactiveCollection<TaskViewModel>();

			Tasks.ItemChanged
				.Throttle(TimeSpan.FromSeconds(5))
				.Select(c => c.Sender)
				.Where(c =>  _repository != null)
				.Where(task => !task.IsEditing)
				.Subscribe(_ => OnSaveAllTasks(null));

			Tasks.ChangeTrackingEnabled = true;

			_bus.Listen<DatabaseChanged>().SubscribeOnDispatcher().Subscribe(OnDatabaseChanged);
			_bus.Listen<CreateNewTask>().SubscribeOnDispatcher().Subscribe(OnCreateNewTask);
			_bus.Listen<SaveAllTasks>().SubscribeOnDispatcher().Subscribe(OnSaveAllTasks);
		}

		public ReactiveCollection<TaskViewModel> Tasks { get; private set; }

		private void OnSaveAllTasks(SaveAllTasks ignored)
		{
			var tasks = Tasks.Select(t => t.Task);
			_repository.SaveAll(tasks);
		}

		

		private void OnDatabaseChanged(DatabaseChanged databaseChanged)
		{
			_repository = NewRepository();

			Tasks.Clear();

			var tasks = _repository.FindAll();

			tasks.Select(t => new TaskViewModel(t)).ForEach(Tasks.Add);

		}

		private void OnCreateNewTask(CreateNewTask ignored)
		{
			var task = new Task();
			Tasks.Add(new TaskViewModel(task));
		}

		private IRepository<Task> NewRepository()
		{
			if (_repository != null)
			{
				_repository.Dispose();
			}
			return _repositoryProvider.Open<Task>();
		}
	}
}
