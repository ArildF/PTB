using System;
using System.Linq;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.Core.Export
{
	public class TaskDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }

		public TaskState State { get; set; }

		public DateTime CreatedDate { get; set; }
		public DateTime ModifiedDate { get; set; }
		public DateTime? StartedDate { get; set; }
		public DateTime? CompletedDate { get; set; }
		public DateTime? AbandonedDate { get; set; }
		public DateTime? StateChangedDate { get; set; }

		public TaskDto()
		{
			
		}

		public void FixUpMissingData()
		{
			if (CreatedDate == default(DateTime))
			{
				CreatedDate = DateTimeHelper.Now;
			}

			if (ModifiedDate == default(DateTime))
			{
				ModifiedDate = DateTimeHelper.Now;
			}

			if (State == TaskState.InProgress && StartedDate == null)
			{
				StartedDate = DateTimeHelper.Now;
			}

			if (State == TaskState.Complete && CompletedDate == null)
			{
				CompletedDate = DateTimeHelper.Now;
			}

			if (State == TaskState.Abandoned && AbandonedDate == null)
			{
				AbandonedDate = DateTimeHelper.Now;
			}

			if (StateChangedDate == null)
			{
				StateChangedDate = new[]{StartedDate, CompletedDate, AbandonedDate}.LastOrDefault(dt => dt != null);
			}
		}
	}
}
