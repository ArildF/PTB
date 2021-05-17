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

		public static int AncestorCount(this Task self)
		{
			return Ancestors(self).Count();
		}

		public static IEnumerable<Task> Ancestors(this Task self)
		{
			Task? current = self;
			while ((current = current.Parent) != null)
			{
				yield return current;
			}
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
	}
}
