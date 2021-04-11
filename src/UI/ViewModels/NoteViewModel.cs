using System;
using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.ViewModels
{
	public class NoteViewModel : ViewModelBase
	{
		private readonly Note _note;

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

	}
}