using System;
using FluentAssertions;
using NUnit.Framework;

namespace Rogue.Ptb.Core.Tests.TaskTests
{
	[TestFixture]
	public class When_creating_a_new_task
	{
		private Task _task;

		[SetUp]
		public void SetUp()
		{
			_task = new Task();
		}

		[Test]
		public void Should_have_CreatedDate_set_to_now()
		{
			_task.CreatedDate.Should().BeWithin(TimeSpan.FromSeconds(0.1)).Before(DateTime.Now);
		}

		[Test]
		public void Should_be_in_state_NotStarted()
		{
			_task.State.Should().Be(TaskState.NotStarted);
		}
	}
}
