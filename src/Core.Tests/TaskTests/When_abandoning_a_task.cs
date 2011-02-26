using System;
using FluentAssertions;
using NUnit.Framework;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.Core.Tests.TaskTests
{
	[TestFixture]
	public class When_abandoning_a_task
	{
		private Task _task;

		[SetUp]
		public void SetUp()
		{
			DateTimeHelper.Fix();

			_task = new Task();

			DateTimeHelper.MoveAheadBy(TimeSpan.FromSeconds(50));

			_task.Start();

			DateTimeHelper.MoveAheadBy(TimeSpan.FromSeconds(50));

			_task.Abandon();

		}

		[Test]
		public void Should_have_AbandonedDate_set_to_now()
		{
			_task.AbandonedDate.Should().Be(DateTimeHelper.Now);
		}

		[Test]
		public void Should_have_ModifiedDate_set_to_now()
		{
			_task.ModifiedDate.Should().Be(DateTimeHelper.Now);
		}

		[Test]
		public void Should_have_StatedChangedDate_set_to_now()
		{
			_task.StateChangedDate.Should().Be(DateTimeHelper.Now);
		}
			
		[Test]
		public void Should_have_state_Abandoned()
		{
			_task.State.Should().Be(TaskState.Abandoned);
		}
	}
}
