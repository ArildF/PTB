using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Iesi.Collections.Generic;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.Core
{
	[DebuggerDisplay("{Title}")]
	public class Task
	{
		private string _title;

		public Task()
		{
			CreatedDate = ModifiedDate = DateTimeHelper.Now;
			Id = Guid.NewGuid();
			_title = "";
			Links = new HashedSet<Link>();
		}

		public virtual Guid Id { get; private set; }

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
			get; private set;

		}

		public virtual DateTime CreatedDate
		{
			get; private set;
		}

		public virtual DateTime ModifiedDate
		{
			get; private set;
		}

		public virtual DateTime? StartedDate
		{
			get; private set;
		}

		public virtual DateTime? CompletedDate
		{
			get; private set;
		}

		protected internal virtual Iesi.Collections.Generic.ISet<Link> Links
		{
			get; private set;
		}

		public virtual DateTime? AbandonedDate { get; private set; }

		public virtual DateTime? StateChangedDate { get; private set; }

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

			Links.Add(new Link(otherTask, type));
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

			return true;
		}
	}
}
