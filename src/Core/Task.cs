using System;

namespace Rogue.Ptb.Core
{
	public class Task
	{
		public Task()
		{
			CreatedDate = DateTime.Now;
		}

		public virtual Guid Id { get; private set; }
		public virtual string Title { get; set; }

		public virtual TaskState State
		{
			get; set;

		}

		public virtual DateTime CreatedDate
		{
			get; private set;
		}
	}
}
