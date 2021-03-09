using FluentAssertions;
using NUnit.Framework;
using Rogue.Ptb.Infrastructure;
using TestUtilities;

namespace Rogue.Ptb.Core.Tests.TaskTests
{
	[TestFixture]
	public class When_creating_a_new_task
	{
		private Task _task;

		[SetUp]
		public void SetUp()
		{
			DateTimeHelper.Reset();
			_task = new Task();
		}

		[Test]
		public void Should_have_CreatedDate_set_to_now()
		{
			_task.CreatedDate.Should().BeAboutNow();
		}

		[Test]
		public void Should_be_in_state_NotStarted()
		{
			_task.State.Should().Be(TaskState.NotStarted);
		}
		
		[Test]
		public void Should_have_ModifiedDate_set_to_now()
		{
			_task.ModifiedDate.Should().BeAboutNow();
		}

		[Test]
		public void Should_not_have_StartedDate()
		{
			_task.StartedDate.Should().Be(null);
		}
			
		
		[Test]
		public void Should_have_state_NotStarted()
		{
			_task.State.Should().Be(TaskState.NotStarted);
		}

		[Test]
		public void Should_have_StatedChangedDate_set_to_null()
		{
			_task.StateChangedDate.Should().Be(null);
		}
	}
}
