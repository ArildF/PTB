using System;
using FluentAssertions;
using NUnit.Framework;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.Core.Tests.TaskTests
{
	[TestFixture]
	public class When_editing_task_title
	{
		private Task _task;

		[SetUp]
		public void SetUp()
		{
			DateTimeHelper.Fix();
			_task = new Task();

			DateTimeHelper.MoveAheadBy(TimeSpan.FromSeconds(20));

			_task.Title = "Blagh";
		}


		[Test]
		public void Should_have_ModifiedDate_set_to_now()
		{
			_task.ModifiedDate.Should().Be(DateTimeHelper.Now);
		}
	}
}
