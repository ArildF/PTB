using System;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.Core
{
	public class Task
	{
		private string _title;

		public Task()
		{
			CreatedDate = ModifiedDate = DateTimeHelper.Now;
			Id = Guid.NewGuid();
			_title = "";
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

		public virtual DateTime? AbandonedDate { get; private set; }

		public virtual DateTime? StateChangedDate { get; private set; }

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

		private void Modified()
		{
			ModifiedDate = DateTimeHelper.Now;
		}
	}
}
