using System;
using System.Reactive;
using System.Reactive.Subjects;
using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.ViewModels
{
	public class NoteViewModel : ViewModelBase
	{
		private readonly Note _note;
		
		private readonly Subject<Unit> _focus = new Subject<Unit>();

		public Subject<Unit> Focus => _focus;

		public NoteViewModel(Note note)
		{
			_note = note;
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

		public DateTime Created => _note.Created;

		public void DoFocus()
		{
			_focus.OnNext(Unit.Default);
		}
	}
}