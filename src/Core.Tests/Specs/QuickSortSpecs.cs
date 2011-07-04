using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace Rogue.Ptb.Core.Tests.Specs
{
	[Subject(typeof (Extensions))]
	public class When_sorting_using_quicksort
	{
		Establish context = () =>
			{
				var random = new Random();
				list = Enumerable.Range(0, 100).Select(_ => random.Next(10000)).ToArray();
			};

		Because of = () => Extensions.QuickSort(list, 0, list.Length - 1, Comparer<int>.Default);

		It should_be_in_sorted_order = () => IsSorted(list);

		private static void IsSorted(int[] ints)
		{
			for (int i = 1; i < ints.Length; i++)
			{
				ints[i].ShouldBeGreaterThanOrEqualTo(ints[i - 1]);
			}
		}

		private static int[] list;
	}
}