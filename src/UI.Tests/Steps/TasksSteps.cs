using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Rogue.Ptb.Core;
using Rogue.Ptb.UI.ViewModels;
using TechTalk.SpecFlow;
using FluentAssertions;
using TestUtilities;

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
			Given("that I have created a new database");

			using (var repos = _context.Get<IRepository<Task>>())
			{
				foreach (var tableRow in table.Rows)
				{
					repos.Save(new Task {Title = tableRow["Title"]});
				}
			}
			_context.Publish(new DatabaseChanged(@"C:\foo\bar.taskboard"));
		}

		[Given(@"a task with the following attributes:")]
		public void GivenATaskWithTheFollowingAttributes(Table table)
		{
			var task = new Task();

			foreach (var row in table.Rows)
			{
				var prop = row["Property"];
				var stringVal = row["Value"];

				var pi = task.GetType().GetProperty(prop, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

				object val;
				if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					var nonNullableType = pi.PropertyType.GetGenericArguments().First();

					val = Convert.ChangeType(stringVal, nonNullableType, CultureInfo.GetCultureInfo("nb-no"));

					val = Activator.CreateInstance(pi.PropertyType, new object[] {val});
				}
				else
				{
					val = Convert.ChangeType(stringVal, pi.PropertyType, CultureInfo.GetCultureInfo("nb-no"));
				}


				pi.SetValue(task, val, null);
			}
			using (var repos = _context.Get<IRepository<Task>>())
			{
				repos.Save(task);
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

		[StepArgumentTransformation]
		public TaskState ConvertToTaskState(string str)
		{
			str = str.Replace(" ", "");

			return (TaskState) Enum.Parse(typeof(TaskState), str);
		}

		[When(@"I drag task \#(\d+) to the ""(.*)"" column")]
		public void WhenIDragTask1ToTheInProgressColumn(int ordinal, TaskState state)
		{
			_context.TaskBoardViewModel.Tasks[ordinal - 1].State = state;
		}


		[Then(@"a new task should be created")]
		public void ThenANewTaskShouldBeCreated()
		{
			_context.TaskBoardViewModel.Tasks.Count.Should().BeGreaterThan(1);
		}

		[Then(@"the new task should have a created date like now")]
		public void ThenTheNewTaskShouldHaveACreatedDateLikeNow()
		{
			_context.TaskBoardViewModel.Tasks.First().Task.CreatedDate.Should().BeAboutNow();

		}

		[Then(@"the new task should have a modified date like now")]
		public void ThenTheNewTaskShouldHaveAModifiedDateLikeNow()
		{
			_context.TaskBoardViewModel.Tasks.First().Task.ModifiedDate.Should().BeAboutNow();
		}

		[Then(@"task \#(\d+) should be ""(.*)""")]
		public void ThenTheTaskShouldBeInProgress(int ordinal, TaskState state)
		{
			_context.TaskBoardViewModel.Tasks[ordinal - 1].Task.State.Should().Be(state);
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

		[Then(@"the tooltip should show")]
		public void ThenTheTooltipShouldShow(string multilineText)
		{
			var expected = NormalizeMultiLineString(multilineText);

			var actual = NormalizeMultiLineString(_context.TaskBoardViewModel.Tasks.First().ToolTip);

			actual.Should().Be(expected);
		}

		private static string NormalizeMultiLineString(string s)
		{
			var lines = from line in s.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
			            select line.Trim();

			return String.Join(Environment.NewLine, lines);
		}


	}
}
