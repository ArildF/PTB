using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Ptb.Core
{
	public class TaskComparer : IComparer<Task>
	{
		private static readonly Dictionary<TaskState, int> SortKeys = new()
		{
			{TaskState.InProgress, 10},
			{TaskState.NotStarted, 8},
			{TaskState.Complete, 5},
			{TaskState.Abandoned, 1}
		};

		public int Compare(Task? x, Task? y)
		{
			var check = (x, y) switch
			{
				(null, null) => 0,
				(null, { }) => 1,
				({ }, null) => -1,
				_ => (int?)null,
			};
			
			if (check != null)
			{
				return check.Value;
			}
			
			if (x!.Parent == y)
			{
				return 1;
			}

			if (y!.Parent == x)
			{
				return -1;
			}

			// the comparands must be on the same level for the comparison to be meaningful
			// walk up the ancestor chain till we find a level where they both have the 
			// same parent (or null).
			if (x.Parent != y.Parent)
			{
				int ancestorCountX = x.AncestorCount();
				int ancestorCountY = y.AncestorCount();
				int diff = ancestorCountX - ancestorCountY;
				if (diff > 0)
				{
					x = x.Ancestors().Skip(diff -1).First();

					// x is child of y, must sort after y
					if (x == y)
					{
						return 1;
					}
				}
				else
				{
					y = y.Ancestors().Skip((-diff) - 1).First();

					// y is child of x, must sort after x
					if (x == y)
					{
						return -1;
					}
				}

				// now x and y are on the same level, and we can compare.
				int retval = Compare(x, y);
				return retval;
			}

			if (x.State != y.State)
			{
				return SortKeys[y.State] - SortKeys[x.State];
			}

			if (x.LessImportantTasksTransitively().Contains(y))
			{
				return -1;
			}

			if (y.LessImportantTasksTransitively().Contains(x))
			{
				return 1;
			}

			int result = CompareByStateAndDates(x, y);
			if (result > 0)
			{
				if (x.LessImportantTasksTransitively().AllLessImportantThanOrIndifferentTo(y))
				{
					return result;
				}
				return -1;
			}
			if (result < 0)
			{
				if (y.LessImportantTasksTransitively().AllLessImportantThanOrIndifferentTo(x))
				{
					return result;
				}
				return 1;
			}
			return result;
		}

		public static int CompareByStateAndDates(Task x, Task y)
		{
			if (x.State != y.State)
			{
				return SortKeys[y.State] - SortKeys[x.State];
			}

			// same state, sort by relevant date
			if (x.State == TaskState.NotStarted)
			{
				return -Comparer<DateTime>.Default.Compare(x.CreatedDate, y.CreatedDate);
			}

			if (x.State == TaskState.InProgress)
			{
				return -Comparer<DateTime?>.Default.Compare(x.StartedDate, y.StartedDate);
			}

			return -Comparer<DateTime?>.Default.Compare(x.StateChangedDate, y.StateChangedDate);
		}
	}
}