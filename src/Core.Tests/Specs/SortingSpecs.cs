using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.Core.Tests.Specs
{
	public class TaskContext
	{
		protected static IList<Task> tasks;

		protected static IList<Task> CreateTasksStaggered(params string[] tasks)
		{
			var list = new List<Task>(tasks.Length);

			foreach (var title in tasks)
			{
				list.Add(new Task {Title = title});
				DateTimeHelper.MoveAheadBy(TimeSpan.FromSeconds(60));
			}
			return list;
		}

		protected static void TaskOrderShouldBe(params string[] titles)
		{
			int idx = 0;
			foreach (var title in tasks.Select(t => t.Title))
			{
				title.ShouldEqual(titles[idx++]);
			}
		}

		protected static Task CreateSubTask(Task task, string title)
		{
			DateTimeHelper.MoveAheadBy(TimeSpan.FromSeconds(60));

			var subTask = task.CreateSubTask();
			subTask.Title = title;
			tasks.Add(subTask);

			return subTask;
		}
	}

	[Subject("Sorting tasks")]
	public class When_sorting_tasks_that_have_relative_priorities_set : TaskContext
	{
		Establish context = () =>
			{
				tasks = CreateTasksStaggered("Task C", "Task B", "Task A", "Task Omega");

				tasks[0].IsMoreImportantThan(tasks[2]);
			};

		Because of = () => tasks.InPlaceSort();

		It should_sort_by_priority_and_then_by_reverse_date_order = () => 
			TaskOrderShouldBe("Task Omega", "Task C", "Task A", "Task B");
	}

	[Subject("Sorting tasks")]
	public class When_sorting_tasks_that_have_multiple_relative_priorities_set : TaskContext
	{
		Establish context = () =>
		{
			tasks = CreateTasksStaggered("One", "Two", "Three");

			tasks[0].IsMoreImportantThan(tasks[2]);
			tasks[1].IsMoreImportantThan(tasks[0]);
		};

		Because of = () => tasks.InPlaceSort();

		It should_sort_by_priority = () =>
			TaskOrderShouldBe("Two", "One", "Three");
	}

	[Subject("Sorting tasks")]
	public class When_sorting_tasks_that_have_relative_priorities_set_and_different_states : TaskContext
	{
		Establish context = () =>
			{
				tasks = CreateTasksStaggered("Task C", "Task B", "Task A", "Task Omega");

				tasks[0].IsMoreImportantThan(tasks[2]);

				tasks[1].Start();
			};

		Because of = () => tasks.InPlaceSort();

		It should_sort_by_state_and_priority_and_then_by_reverse_date_order = () => 
			TaskOrderShouldBe("Task B", "Task Omega", "Task C", "Task A");
	}

	[Subject("Sorting tasks")]
	public class When_sorting_tasks_that_have_different_states : TaskContext
	{
		Establish context = () =>
			{
				tasks = CreateTasksStaggered("Task 1", "Task 2", "Task 3");
				
				tasks[2].Start();
				tasks[1].Start();
				tasks[1].Complete();

			};

		Because of = () => tasks.InPlaceSort();

		It should_sort_the_in_progress_task_first = () => tasks.First().Title.ShouldEqual("Task 3");

		It should_sort_the_completed_task_last = () => tasks.Last().Title.ShouldEqual("Task 2");
	}

	[Subject("Sorting tasks")]
	public class When_sorting_tasks_with_the_same_state_and_no_dependencies : TaskContext
	{
		Establish context = () =>
		{
			tasks = CreateTasksStaggered("Task 3", "Task 1", "Task 2");
		};

		Because of = () => tasks.InPlaceSort();

		private It should_sort_the_tasks_by_creation_date_with_latest_first =
			() => TaskOrderShouldBe("Task 2", "Task 1", "Task 3");

	}

	[Subject("Sorting tasks")]
	public class When_sorting_tasks_with_subtasks : TaskContext
	{
		Establish context = () =>
		{
			tasks = CreateTasksStaggered("Task 3", "Task 1", "Task 2");
			CreateSubTask(tasks[1], "Task 1a");

			CreateSubTask(tasks[0], "Task 3b");
			CreateSubTask(tasks[0], "Task 3a");

			CreateSubTask(tasks[2], "Task 2c");
			CreateSubTask(tasks[2], "Task 2b");
			CreateSubTask(tasks[2], "Task 2a");

		};

		Because of = () => tasks.InPlaceSort();

		private It should_sort_the_subtasks_with_their_parents_but_ordered_by_create_date_descending = 
			() => TaskOrderShouldBe(
				"Task 2", "Task 2a", "Task 2b", "Task 2c",
				"Task 1", "Task 1a", 
				"Task 3", "Task 3a", "Task 3b", "Task 3c");

	}

	[Subject("Sorting tasks")]
	public class When_sorting_tasks_with_subtasks_of_subtasks : TaskContext
	{
		Establish context = () =>
			{
				tasks = CreateTasksStaggered("1");
				var one = tasks[0];

				CreateSubTask(one, "1c");
				CreateSubTask(one, "1b");
				var oneA = CreateSubTask(one, "1a");
				CreateSubTask(oneA, "1a1");

			};

		Because of = () => tasks.InPlaceSort();

		It should_sort_subtasks_with_their_parents = () => TaskOrderShouldBe(
			"1",
				"1a",
					"1a1",
				"1b",
				"1c");
	}

}