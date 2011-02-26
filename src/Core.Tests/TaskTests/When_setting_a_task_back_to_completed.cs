using System;
using FluentAssertions;
using NUnit.Framework;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.Core.Tests.TaskTests
{
	[TestFixture]
	public class When_setting_a_task_back_to_completed_from_abandoned
	{
		protected Task _task;

		[SetUp]
		public void SetUp()
		{
			DateTimeHelper.Fix();

			_task = new Task();

			GoToInitialState(_task);


			DateTimeHelper.MoveAheadBy(TimeSpan.FromSeconds(50));

			_task.Complete();
		}

		protected virtual void GoToInitialState(Task task)
		{
			task.Abandon();
		}

		[Test]
		public void Should_have_state_InProgress()
		{
			_task.State.Should().Be(TaskState.Complete);
		}

		[Test]
		public void Should_have_ModifiedDate_set_to_now()
		{
			_task.ModifiedDate.Should().Be(DateTimeHelper.Now);
		}

		[Test]
		public void Should_have_AbandonedDate_set_to_null()
		{
			_task.AbandonedDate.Should().Be(null);
		}
	}
}