using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Services;

namespace Rogue.Ptb.UI.ViewModels
{
	public class NotesDisplayViewModel : ViewModelBase
	{
		private readonly Task _task;
		private readonly IEventAggregator _bus;
		private readonly IMessageBoxService _messageBoxService;
		private readonly INoteViewModelFactory _noteViewModelFactory;
		private NoteViewModel _selectedNoteViewModel;

		public NotesDisplayViewModel(Task task, IEventAggregator bus, IMessageBoxService messageBoxService,
			INoteViewModelFactory noteViewModelFactory)
		{
			_task = task;
			_bus = bus;
			_messageBoxService = messageBoxService;
			_noteViewModelFactory = noteViewModelFactory;
			Notes = new ObservableCollectionExtended<NoteViewModel>();
			PopulateNotes();

			bus.AddSource(Notes.ToObservableChangeSet()
				.WhenAnyPropertyChanged().Select(_ => Unit.Default)
				.Merge(Notes.ObserveCollectionChanges().Select(_ => Unit.Default))
				.Throttle(TimeSpan.FromSeconds(5))
				.Select(_ => new TaskModified()));

			StartEditingCommand = ReactiveCommand.Create(() =>
			{
				IsEditing = true;
				SelectedNoteViewModel?.DoFocus();
				return true;
			});
			EndEditingCommand = ReactiveCommand.Create(() => IsEditing = false);

			AddNoteCommand = ReactiveCommand.Create(AddNote);

			DeleteCommand = ReactiveCommand.Create<NoteViewModel>(DeleteNote);

			this.ObservableForProperty(vm => vm.ShowSubNotes)
				.ObserveOnDispatcher()
				.Subscribe(_ => PopulateNotes());

		}

		private void PopulateNotes()
		{
			Notes.Clear();
			
			var tasks = _showSubNotes
				? _task.RecurseDepthFirst(t => t.SubTasks)
				: _task.AsSingleItemEnumerable();
			Notes.AddRange(
				from t in tasks
				from n in t.Notes
				orderby n.Created descending 
				select _noteViewModelFactory.CreateNoteViewModel(n, t));
			
			if (!Notes.Any())
			{
				Notes.Add(_noteViewModelFactory.CreateNoteViewModel(_task.CreateNote(), _task));
			}

			SelectedNoteViewModel = Notes.First();
		}

		private bool _showSubNotes;

		public bool ShowSubNotes
		{
			get => _showSubNotes;
			set => this.RaiseAndSetIfChanged(ref _showSubNotes, value);
		}
		

		public ReactiveCommand<NoteViewModel, Unit> DeleteCommand { get; set; }

		private void DeleteNote(NoteViewModel obj)
		{
			if (_messageBoxService.ShowYesNoDialog("Delete note?") == MessageBoxResult.Yes)
			{
				var nextNote = Notes.SkipWhile(n => n != obj).Skip(1).FirstOrDefault();
				_task.Notes.Remove(obj.Note);
				Notes.Remove(obj);
				SelectedNoteViewModel = nextNote ?? Notes.Last();
			}
			
		}

		public ICommand AddNoteCommand { get; }

		private void AddNote()
		{
			var note = _task.CreateNote();
			var noteViewModel = _noteViewModelFactory.CreateNoteViewModel(note, _task);
			Notes.Insert(0, noteViewModel);

			SelectedNoteViewModel = noteViewModel;
		}


		public ICommand EndEditingCommand { get; }

		public ICommand StartEditingCommand { get; }

		public NoteViewModel SelectedNoteViewModel
		{
			get => _selectedNoteViewModel; 
			set => this.RaiseAndSetIfChanged(ref _selectedNoteViewModel, value);
		}

		private bool _isEditing;

		public bool IsEditing
		{
			get => _isEditing;
			set => this.RaiseAndSetIfChanged(ref _isEditing, value);
		}

		public string Title => _task.Title;

		public ObservableCollectionExtended<NoteViewModel> Notes
		{
			get;
		}
	}
}