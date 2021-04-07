using System;
using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.Views
{
	public class NoteDisplayer
	{
		private readonly Func<Task, NotesWindow> _factory;

		public NoteDisplayer(Func<Task, NotesWindow> factory)
		{
			_factory = factory;
		}

		public void Display(Task task)
		{
			var window = _factory(task);
			window.Show();
		}
	}
}