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
	public class TasksSteps : TechTalk.SpecFlow.Steps
	{
		private readonly Context _context;

		public TasksSteps(Context context)
		{
			_context = context;
		}

		[Given(@"that the following tasks already exist and are loaded:")]
		[When(@"that the following tasks already exist and are loaded:")]
		public void GivenThatTheFollowingTasksAlreadyExist(Table table)
		{
			using (var repos = _context.Get<IRepository<Task>>())
			{
				foreach (var tableRow in table.Rows)
				{
					repos.Save(new Task {Title = tableRow["Title"]});
				}
			}
			_context.Publish(new DatabaseChanged(@"C:\foo\bar.taskboard"));
		}

		[Given(@"that the following tasks already exist and are exported to ""(.*)"":")]
		public void GivenThatTheFollowingTasksAlreadyExistAndAreExportedToCFooImport_Taskboard(string path, Table table)
		{
			GivenThatTheFollowingTasksAlreadyExist(table);
			Given(String.Format(@"that I enter ""{0}"" in the export taskboard dialog", path));
			When("I click export task");
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

		[Then(@"the new task should have a created date like now")]
		public void ThenTheNewTaskShouldHaveACreatedDateLikeNow()
		{
			_context.TaskBoardViewModel.Tasks.First().Task.CreatedDate.Should().BeWithin(TimeSpan.FromSeconds(1)).Before(
				DateTime.Now);

		}


		[Then(@"the database should contain (\d+) tasks")]
		public void ThenTheDatabaseShouldContain2Tasks(int count)
		{
			using (var repos = _context.Get<IRepository<Task>>())
			{
				repos.FindAll().Count().Should().Be(count);
			}
		}

		[Then(@"the taskboard should contain these tasks:")]
		public void ThenTheTaskboardShouldContainTheseTasks(Table table)
		{
			_context.TaskBoardViewModel.Tasks.Select(t => t.Title).Should().BeEquivalentTo(
				table.Rows.Select(r => r["Title"]));
		}

		[Then(@"the new task should be displayed first")]
		public void ThenTheNewTaskShouldBeDisplayedFirst()
		{
			_context.TaskBoardViewModel.Tasks.First().Task.Title.Should().Be(String.Empty);
		}

		[Then(@"the new task should be in edit mode")]
		public void ThenTheNewTaskShouldBeInEditMode()
		{
			_context.TaskBoardViewModel.Tasks.First(t => String.IsNullOrEmpty(t.Title)).IsEditing.Should().BeTrue();

		}

		[Then(@"the task should not be started")]
		public void ThenTheTaskShouldNotBeStarted()
		{
			_context.TaskBoardViewModel.Tasks.First().Task.State.Should().Be(TaskState.NotStarted);
		}

	}
}
