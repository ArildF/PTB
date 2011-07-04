using NUnit.Framework;
using FluentAssertions;

namespace Rogue.Ptb.Core.Tests.TaskTests
{
	[TestFixture]
	public class When_setting_a_task_to_be_more_important_than_another_task
	{
		private Task _otherTask;
		private Task _task;

		[SetUp]
		public void SetUp()
		{
			_task = new Task();

			_otherTask = new Task();

			_task.IsMoreImportantThan(_otherTask);
		}

		[Test]
		public void Should_be_more_important_than_the_other_task()
		{
			_task.LessImportantTasks.Should().Contain(_otherTask);
		}

		[Test]
		public void Should_have_other_task_be_less_important()
		{
			_otherTask.MoreImportantTasks.Should().Contain(_task);
		}
	}
}
