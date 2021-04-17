using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.Core
{
	[DebuggerDisplay("{Title}: Id: {Id}")]
	public class Task
	{
		private string _title;

		public Task()
		{
			CreatedDate = ModifiedDate = DateTimeHelper.Now;
			Id = Guid.NewGuid();
			_title = "";
			Links = new HashSet<Link>();
			Notes = new HashSet<Note>();
		}

		public virtual Guid Id { get; protected internal set; }

		public virtual string Title
		{
			get { return _title; }
			set
			{
				_title = value;
				Modified();
			}
		}

		public virtual TaskState State
		{
			get; protected internal set ;

		}

		public virtual DateTime CreatedDate
		{
			get; protected internal set;
		}

		public virtual DateTime ModifiedDate
		{
			get; protected internal set;
		}

		public virtual DateTime? StartedDate
		{
			get; protected internal set;
		}

		public virtual DateTime? CompletedDate
		{
			get; protected internal set;
		}
		
		public virtual ISet<Note> Notes { get; set; }

		protected internal virtual ISet<Link> Links
		{
			get; set;
		}

		public virtual DateTime? AbandonedDate { get; protected internal set; }

		public virtual DateTime? StateChangedDate { get; protected internal set; }

		public virtual IEnumerable<Task> LessImportantTasks
		{
			get
			{
				return Links.Where(link => link.Type == LinkType.MoreImportantThan)
					.Select(link => link.LinkTo);
			}
		}

		public virtual IEnumerable<Task> MoreImportantTasks
		{
			get
			{
				return FindRelatedTasks(LinkType.LessImportantThan);
			}
		}

		public virtual IEnumerable<Task> SubTasks
		{
			get { return FindRelatedTasks(LinkType.Child); }
		}

		public virtual Task Parent
		{
			get { return FindRelatedTasks(LinkType.Parent).FirstOrDefault(); }
		}

		public virtual bool IsLeaf => !SubTasks.Any();
		
		public virtual double? Progress { get; set; }


		public override string ToString()
		{
			return Title;
		}

		public virtual void Start()
		{
			State = TaskState.InProgress;
			StartedDate = DateTimeHelper.Now;

			CompletedDate = null;
			AbandonedDate = null;

			StateChanged();

			if (Parent != null)
			{
				Parent.Start();
			}
		}

		public virtual void Complete()
		{
			State = TaskState.Complete;
			CompletedDate = DateTimeHelper.Now;
			AbandonedDate = null;

			StateChanged();
		}

		public virtual void Abandon()
		{
			State = TaskState.Abandoned;
			AbandonedDate = DateTimeHelper.Now;

			StateChanged();
		}

		public virtual void NotStarted()
		{
			State = TaskState.NotStarted;

			StartedDate = null;
			CompletedDate = null;
			AbandonedDate = null;

			StateChanged();
		}

		private void StateChanged()
		{
			StateChangedDate = DateTimeHelper.Now;
			Modified();
		}

		private IEnumerable<Task> FindRelatedTasks(LinkType linkType)
		{
			return Links.Where(link => link.Type == linkType)
				.Select(link => link.LinkTo);
		}

		private void Modified()
		{
			ModifiedDate = DateTimeHelper.Now;
		}

		public virtual void IsMoreImportantThan(Task otherTask)
		{
			if (!CanBeMoreImportantThan(otherTask))
			{
				throw new InvalidOperationException(String.Format("Cannot make {0} more important than {1}", this, otherTask));
			}
			AddLink(otherTask, LinkType.MoreImportantThan);
			otherTask.AddLink(this, LinkType.LessImportantThan);
		}

		private void AddLink(Task otherTask, LinkType type)
		{
			var existingLink = Links.FirstOrDefault(link => link.LinkTo == otherTask);

			if (existingLink != null)
			{
				if (existingLink.Type == type)
				{
					return;
				}
				Links.Remove(existingLink);
			}

			Links.Add(new Link(this, otherTask, type));
		}

		public virtual void IsLessImportantThan(Task otherTask)
		{
			AddLink(otherTask, LinkType.LessImportantThan);
			otherTask.AddLink(this, LinkType.MoreImportantThan);
		}

		public virtual Task CreateSubTask()
		{
			var subTask = new Task();
			AddLink(subTask, LinkType.Child);
			subTask.AddLink(this, LinkType.Parent);

			return subTask;
		}

		public virtual bool CanBeMoreImportantThan(Task otherTask)
		{
			if (Parent != null && otherTask.Parent == null)
			{
				return false;
			}
			if (otherTask.LessImportantTasks.Contains(this))
			{
				return true;
			}

			if (otherTask.LessImportantTasksTransitively().FirstOrDefault(t => t == this) != null)
			{
				return false;
			}

			return true;
		}

		public virtual Note CreateNote()
		{
			var note = new Note {Created = DateTime.Now, Markdown = ""};
			Notes.Add(note);
			return note;
		}
	}
}
