using System;
using System.Collections.ObjectModel;
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

			Tasks = new ObservableCollection<TaskViewModel>();
			_bus.Listen<DatabaseChanged>().SubscribeOnDispatcher().Subscribe(OnDatabaseChanged);
			_bus.Listen<CreateNewTask>().SubscribeOnDispatcher().Subscribe(OnCreateNewTask);
			_bus.Listen<SaveAllTasks>().SubscribeOnDispatcher().Subscribe(OnSaveAllTasks);


		}

		public ObservableCollection<TaskViewModel> Tasks { get; private set; }

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
