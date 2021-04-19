using System;
using System.Reactive;
using System.Reactive.Subjects;
using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.ViewModels
{
	public class NoteViewModel : ViewModelBase
	{
		private readonly Note _note;
		private readonly Task _task;

		private readonly Subject<Unit> _focus = new();

		public Subject<Unit> Focus => _focus;

		public NoteViewModel(Note note, Task task)
		{
			_note = note;
			_task = task;
		}

		public string Markdown
		{
			get => _note.Markdown;
			set
			{
				_note.Markdown = value;
				this.RaisePropertyChanged(vm => vm.Markdown);
			}
		}

		public string Title => _task.Title;

		public DateTime Created => _note.Created;
		public Note Note => _note;

		public void DoFocus()
		{
			_focus.OnNext(Unit.Default);
		}
	}
}