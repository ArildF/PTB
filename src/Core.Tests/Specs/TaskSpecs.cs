using System;

using Machine.Specifications;

namespace Rogue.Ptb.Core.Tests.Specs
{
	[Subject(typeof (Task))]
	public class When_adding_a_subtask_to_a_task
	{
		private Establish context = () =>
			{
				task = new Task();
			};

		private Because of = () => subTask = task.CreateSubTask();

		private It should_have_the_subtask_as_a_child = () => task.SubTasks.ShouldContain(subTask);

		private It should_have_the_subtask_have_itself_as_its_parent = () => subTask.Parent.ShouldEqual(task);

		private It should_not_allow_the_subtask_to_be_more_important_than_itself = () =>
			subTask.CanBeMoreImportantThan(task).ShouldBeFalse();

		private It should_throw_if_attempting_to_make_the_subtask_more_important_than_itself = () =>
			Catch.Exception(() => subTask.IsMoreImportantThan(task)).ShouldBeOfExactType<InvalidOperationException>();

		private static Task task;
		private static Task subTask;
	}

	public class When_setting_a_task_more_important_than_a_task_that_is_transitively_more_important_than_itself : TaskContext
	{
		Establish context = () =>
			{
				tasks = CreateTasksStaggered("Task3", "Task2", "Task1");
				tasks[2].IsMoreImportantThan(tasks[1]);
				tasks[1].IsMoreImportantThan(tasks[0]);
			};

		Because of = () => result = tasks[0].CanBeMoreImportantThan(tasks[2]);

		It should_not_be_allowed = () => result.ShouldBeFalse();

		private static bool result;
	}

	public class When_starting_a_subtask_of_another_task
	{
		Establish context = () =>
			{
				parent = new Task();
				subTask = parent.CreateSubTask();
			};

		Because of = () => subTask.Start();

		It should_then_start_the_parent_task = () => parent.State.ShouldEqual(TaskState.InProgress);

		private static Task parent;
		private static Task subTask;
	}
}