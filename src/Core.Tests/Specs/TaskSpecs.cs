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
			Catch.Exception(() => subTask.IsMoreImportantThan(task)).ShouldBeOfType<InvalidOperationException>();

		private static Task task;
		private static Task subTask;
	}
}