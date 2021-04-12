using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.ViewModels
{
	public class NotesDisplayViewModel : ViewModelBase
	{
		private readonly Task _task;
		private NoteViewModel _selectedNoteViewModel;

		public NotesDisplayViewModel(Task task, IEventAggregator bus)
		{
			_task = task;
			Notes = new ObservableCollectionExtended<NoteViewModel>(task.Notes.Select(n => new NoteViewModel(n)));
			if (!Notes.Any())
			{
				Notes.Add(new NoteViewModel(task.CreateNote()));
			}

			bus.AddSource(Notes.ToObservableChangeSet()
				.WhenAnyPropertyChanged()
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

			SelectedNoteViewModel = Notes.First();
		}

		public ICommand AddNoteCommand { get; }

		private void AddNote()
		{
			var note = _task.CreateNote();
			Notes.Add(new NoteViewModel(note));
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