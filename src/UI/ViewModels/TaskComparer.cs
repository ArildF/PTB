using System;
using System.Collections.Generic;
using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.ViewModels
{
	public class TaskComparer : IComparer<TaskViewModel>
	{
		private static readonly Dictionary<TaskState, int> _sortKeys = new Dictionary<TaskState, int>
			{
				{TaskState.InProgress, 10},
				{TaskState.NotStarted, 8},
				{TaskState.Complete, 5},
				{TaskState.Abandoned, 1}
			};

		public int Compare(TaskViewModel x, TaskViewModel y)
		{
			var task1 = x.Task;
			var task2 = y.Task;

			if (task1.State != task2.State)
			{
				return _sortKeys[task2.State] - _sortKeys[task1.State];
			}

			// same state, sort by relevant date
			if (task1.State == TaskState.NotStarted)
			{
				return -Comparer<DateTime>.Default.Compare(task1.CreatedDate, task2.CreatedDate);
			}

			if (task1.State == TaskState.InProgress)
			{
				return -Comparer<DateTime?>.Default.Compare(task1.StartedDate, task2.StartedDate);
			}

			return -Comparer<DateTime?>.Default.Compare(task1.StateChangedDate, task2.StateChangedDate);

		}
	}
}
