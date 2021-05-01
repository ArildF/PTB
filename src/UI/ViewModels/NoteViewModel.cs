using System;
using System.Reactive;
using System.Reactive.Subjects;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Behaviors;

namespace Rogue.Ptb.UI.ViewModels
{
	public class NoteViewModel : ViewModelBase, IAmMarkdownNote
	{
		private readonly Note _note;
		private readonly Task _task;
		private readonly IEventAggregator _bus;

		private readonly Subject<Unit> _focus = new();

		public Subject<Unit> Focus => _focus;

		public NoteViewModel(Note note, Task task, IEventAggregator bus)
		{
			_note = note;
			_task = task;
			_bus = bus;
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

		public async System.Threading.Tasks.Task AddAttachment(Attachment attachment)
		{
			_note.AddAttachment(attachment);
			await _bus.PublishAndWait<TaskModified, TasksSaved>(new TaskModified());
		}
	}
}