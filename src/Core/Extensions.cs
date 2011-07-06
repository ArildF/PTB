using System;
using System.Collections.Generic;
using System.Linq;

namespace Rogue.Ptb.Core
{
	public static class Extensions
	{
		public static void InPlaceSort(this IList<Task> self)
		{
			QuickSort(self, 0, self.Count -1, new TaskComparer());

		}

		public static bool AllLessImportantThanOrIndifferentTo(this IEnumerable<Task> self, Task comparee)
		{
			return self.All(t => LessImportantThanOrIndifferentTo(t, comparee));
		}

		public static IEnumerable<Task> LessImportantTasksTransitively(this Task self)
		{
			foreach (var lessImportantTask in self.LessImportantTasks)
			{
				yield return lessImportantTask;
				foreach (var task in lessImportantTask.LessImportantTasksTransitively())
				{
					yield return task;
				}
			}
		}

		private static bool LessImportantThanOrIndifferentTo(Task task, Task comparee)
		{
			int result = TaskComparer.CompareByStateAndDates(comparee, task);
			return result <= 0;
		}

		public static void QuickSort<T>(IList<T> self, int left, int right, IComparer<T> comparer)
		{
			if (left >= right)
			{
				return;
			}

			int pivot = (left + right)/2;
			int newPivot = Partition(self, left, right, pivot, comparer);
			QuickSort(self, left, newPivot -1, comparer);
			QuickSort(self, newPivot + 1, right, comparer);
		}

		private static int Partition<T>(IList<T> self, int left, int right, int pivot, IComparer<T> comparer)
		{
			var pivotValue = self[pivot];
			self.Swap(pivot, right);
			int index = left;

			for (int i = left; i <= right - 1; i++)
			{
				if (comparer.Compare(self[i], pivotValue) < 0)
				{
					self.Swap(i, index);
					index++;
				}
			}

			self.Swap(index, right);
			return index;
		}

		private static void Swap<T>(this IList<T> self, int idx1, int idx2)
		{
			var temp = self[idx1];
			self[idx1] = self[idx2];
			self[idx2] = temp;
		}

		public class TaskComparer : IComparer<Task>
		{
			private static readonly Dictionary<TaskState, int> _sortKeys = new Dictionary<TaskState, int>
			{
				{TaskState.InProgress, 10},
				{TaskState.NotStarted, 8},
				{TaskState.Complete, 5},
				{TaskState.Abandoned, 1}
			};

			public int Compare(Task x, Task y)
			{
				if (x.Parent == y)
				{
					return 1;
				}

				if (y.Parent == x)
				{
					return -1;
				}

				if (x.Parent != y.Parent)
				{
					return Compare(x.Parent ?? x, y.Parent ?? y);
				}

				if (x.State != y.State)
				{
					return _sortKeys[y.State] - _sortKeys[x.State];
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
					return _sortKeys[y.State] - _sortKeys[x.State];
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
}
