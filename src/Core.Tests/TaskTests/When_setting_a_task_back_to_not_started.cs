using System;
using FluentAssertions;
using NUnit.Framework;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.Core.Tests.TaskTests
{
	[TestFixture]
	public class When_setting_a_task_back_to_not_started_from_Started
	{
		protected Task _task;

		[SetUp]
		public void SetUp()
		{
			DateTimeHelper.Fix();

			_task = new Task();

			GoToInitialState(_task);


			DateTimeHelper.MoveAheadBy(TimeSpan.FromSeconds(50));

			_task.NotStarted();
		}

		protected virtual void GoToInitialState(Task task)
		{
			task.Start();
		}

		[Test]
		public void Should_have_state_NotStarted()
		{
			_task.State.Should().Be(TaskState.NotStarted);
		}

		[Test]
		public void Should_have_ModifiedDate_set_to_now()
		{
			_task.ModifiedDate.Should().Be(DateTimeHelper.Now);
		}

		[Test]
		public void Should_have_StartedDate_set_to_null()
		{
			_task.StartedDate.Should().Be(null);
		}

	}

	[TestFixture]
	public class When_setting_a_task_back_to_not_started_from_Complete : When_setting_a_task_back_to_not_started_from_Started
	{
		protected override void GoToInitialState(Task task)
		{
			task.Complete();
		}

		[Test]
		public void Should_have_CompletedDate_set_to_null()
		{
			_task.CompletedDate.Should().Be(null);
		}

	}
	[TestFixture]
	public class When_setting_a_task_back_to_not_started_from_Abandoned : When_setting_a_task_back_to_not_started_from_Complete
	{
		protected override void GoToInitialState(Task task)
		{
			task.Abandon();
		}

		[Test]
		public void Should_have_AbandonedDate_set_to_null()
		{
			_task.AbandonedDate.Should().Be(null);
		}

	}
}
