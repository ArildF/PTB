using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rogue.Ptb.Core;
using Rogue.Ptb.UI.ViewModels;
using TechTalk.SpecFlow;
using FluentAssertions;

namespace Rogue.Ptb.UI.Tests.Steps
{
	[Binding]
	public class TasksSteps
	{
		private readonly Context _context;

		public TasksSteps(Context context)
		{
			_context = context;
		}

		[Given(@"that the following tasks already exist and are loaded:")]
		public void GivenThatTheFollowingTasksAlreadyExist(Table table)
		{
			using (var repos = _context.Get<IRepository<Task>>())
			{
				foreach (var tableRow in table.Rows)
				{
					repos.Save(new Task {Title = tableRow["Title"]});
				}
			}
			_context.Publish(new DatabaseChanged());
		}

		[When(@"I click new task")]
		public void WhenIClickNewTask()
		{
			_context.Publish(new CreateNewTask());

		}

		[Then(@"a new task should be created")]
		public void ThenANewTaskShouldBeCreated()
		{
			_context.TaskBoardViewModel.Tasks.Count.Should().BeGreaterThan(1);
		}

		[Then(@"the database should contain (\d+) tasks")]
		public void ThenTheDatabaseShouldContain2Tasks(int count)
		{
			using (var repos = _context.Get<IRepository<Task>>())
			{
				repos.FindAll().Count().Should().Be(count);
			}
		}

		[Then(@"the new task should be displayed first")]
		public void ThenTheNewTaskShouldBeDisplayedFirst()
		{
			_context.TaskBoardViewModel.Tasks.First().Task.Id.Should().Be(Guid.Empty);
		}
	}
}
