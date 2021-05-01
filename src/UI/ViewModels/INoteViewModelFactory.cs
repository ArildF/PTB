using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.ViewModels
{
	public interface INoteViewModelFactory
	{
		NoteViewModel CreateNoteViewModel(Note note, Task task);
	}
}