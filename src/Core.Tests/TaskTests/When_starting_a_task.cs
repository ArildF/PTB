using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rogue.Ptb.Infrastructure;
using FluentAssertions;
using TestUtilities;

namespace Rogue.Ptb.Core.Tests.TaskTests
{
	[TestFixture]
	public class When_starting_a_task
	{
		private Task _task;
		private DateTime _startedOn;

		[SetUp]
		public void SetUp()
		{
			var dateTime = DateTime.Now;
			DateTimeHelper.SetNow(dateTime);

			_startedOn = dateTime;

			_task = new Task();

			DateTimeHelper.SetNow(dateTime + TimeSpan.FromMinutes(5));

			_task.Start();
		}

		[Test]
		public void Should_have_ModifiedDate_set_to_now()
		{
			_task.ModifiedDate.Should().Be(DateTimeHelper.Now);
		}

		[Test]
		public void Should_have_StartedDate_set_to_now()
		{
			_task.StartedDate.Should().Be(DateTimeHelper.Now);
		}

		[Test]
		public void Should_have_CreatedDate_set_in_the_past()
		{
			_task.CreatedDate.Should().Be(_startedOn);
		}

		[Test]
		public void Should_have_StatedChangedDate_set_to_now()
		{
			_task.StateChangedDate.Should().Be(DateTimeHelper.Now);
		}

		[Test]
		public void Should_have_state_InProgress()
		{
			_task.State.Should().Be(TaskState.InProgress);
		}


	}
}
