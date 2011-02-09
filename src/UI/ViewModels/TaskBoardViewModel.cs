using System;
using System.Linq;
using ReactiveUI;
using Rogue.Ptb.Core;
using NHibernate.Linq;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.ViewModels
{
	public class TaskBoardViewModel : ViewModelBase, ITaskBoardViewModel
	{
		private readonly IRepositoryProvider _repositoryProvider;
		private Core.IRepository<Task> _repository;
		private readonly IEventAggregator _bus;

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

			_bus.ListenOnScheduler<DatabaseChanged>(OnDatabaseChanged);
			_bus.ListenOnScheduler<CreateNewTask>(OnCreateNewTask);
			_bus.ListenOnScheduler<SaveAllTasks>(OnSaveAllTasks);
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

			TypeHelperExtensionMethods.ForEach(tasks.Select(t => new TaskViewModel(t)), Tasks.Add);

		}

		private void OnCreateNewTask(CreateNewTask ignored)
		{
			var task = new Task(){Title = "New Task"};
			Tasks.Insert(0, new TaskViewModel(task));
		}

		private Core.IRepository<Task> NewRepository()
		{
			if (_repository != null)
			{
				_repository.Dispose();
			}
			return _repositoryProvider.Open<Task>();
		}
	}
}
