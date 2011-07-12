using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;
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
					DateTimeHelper.MoveAheadBy(TimeSpan.FromSeconds(-5));
					repos.InsertNew(new Task {Title = tableRow["Title"]});
				}
			}
			_context.Publish(new DatabaseChanged(@"C:\foo\bar.taskboard"));
		}

		[Given(@"that task \#(\d+) has a subtask ""(.*)""")]
		public void GivenThatTask1HasASubtaskSubA(int num, string title)
		{
			_context.TaskBoardViewModel.SelectedTask = _context.TaskByOrdinal(num);
			_context.Publish<CreateNewSubTask>(null);
			_context.TaskBoardViewModel.SelectedTask.Title = title;
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

		[When(@"I select task \#(\d+)")]
		public void WhenISelectTask1(int num)
		{
			_context.TaskBoardViewModel.Tasks.Skip(num - 1).First().Select();
		}

		[When(@"click new subtask")]
		public void WhenClickNewSubtask()
		{
			_context.Publish<CreateNewSubTask>(null);
		}

		[When(@"I select task '(.*)'")]
		public void WhenISelectTaskYo(string taskTitle)
		{
			var vm = _context.FindTaskVM(taskTitle);

			_context.TaskBoardViewModel.SelectedTask = vm;
		}


		[When(@"I add a subtask ""(.*)"" to task ""(.*)""")]
		[Given(@"I add a subtask ""(.*)"" to task ""(.*)""")]
		public void WhenIAddASubtaskThree_AToTaskThree(string subtaskName, string task)
		{
			var vm = _context.FindTaskVM(task);

			_context.TaskBoardViewModel.SelectedTask = vm;
			_context.Publish<CreateNewSubTask>(null);

			_context.TaskBoardViewModel.SelectedTask.Title = subtaskName;
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

		[When(@"I collapse the hierarchy for task ""(.*)""")]
		[When(@"I expand the hierarchy for task ""(.*)""")]
		public void WhenICollapseTheHierarchyForTaskTwo(TaskViewModel vm)
		{
			vm.ToggleCollapseHierarchyCommand.Execute(null);
		}


		[Then(@"a new task should be created")]
		public void ThenANewTaskShouldBeCreated()
		{
			_context.TaskBoardViewModel.Tasks.Count.Should().BeGreaterThan(1);
		}

		[Then(@"task ""(.*)"" should be collapsable")]
		public void ThenTaskTwoShouldBeCollapsable(string title)
		{
			var vm = _context.FindTaskVM(title);
			vm.CanCollapse.Should().BeTrue();
		}

		[Then(@"task ""(.*)"" should be expandable")]
		public void ThenTaskTwoShouldBeExpandable(string title)
		{
			var vm = _context.FindTaskVM(title);
			vm.CanExpand.Should().BeTrue();
		}

		[Then(@"the new task should be in position \#(\d+)")]
		public void ThenTheNewTaskShouldBeInPosition2(int pos)
		{
			_context.TaskBoardViewModel.Tasks.Select((vm, index) => new {Vm = vm, Index = index})
				.Where(obj => obj.Vm == _context.NewestTask)
				.Select(obj => obj.Index).First().Should().Be(pos - 1);

		}

		[Then(@"the new task should have a created date like now")]
		public void ThenTheNewTaskShouldHaveACreatedDateLikeNow()
		{
			_context.NewestTask.Task.CreatedDate.Should().BeAboutNow();
		}

		[Then(@"task \#(\d+) should be in edit mode")]
		public void ThenTask2ShouldBeInEditMode(int num)
		{
			_context.TaskByOrdinal(num).IsEditing.Should().BeTrue();
		}

		[Then(@"task \#(\d+) should be selected")]
		public void ThenTask1ShouldBeSelected(int num)
		{
			_context.TaskByOrdinal(num).Should().Be(_context.TaskBoardViewModel.SelectedTask);
			_context.TaskByOrdinal(num).IsSelected.Should().BeTrue();
		}

		[When(@"I begin editing task \#(\d+)")]
		public void WhenIBeginEditingTask2(int num)
		{
			_context.TaskByOrdinal(num).BeginEdit();
		}

		[When(@"I deselect all")]
		public void WhenIDeselectAll()
		{
			_context.TaskBoardViewModel.Deselect();
		}


		[Then(@"task \#(\d+) should not be selected")]
		public void ThenTask1ShouldNotBeSelected(int num)
		{
			_context.TaskByOrdinal(num).IsSelected.Should().Be(false);
		}


		[Then(@"the new task should have a modified date like now")]
		public void ThenTheNewTaskShouldHaveAModifiedDateLikeNow()
		{
			_context.NewestTask.Task.ModifiedDate.Should().BeAboutNow();
		}

		[Then(@"task ""(.*)"" should show a collapse button")]
		public void ThenTaskTwoShouldShowACollapseButton(TaskViewModel vm)
		{
			vm.Collapsable.Should().BeTrue();
		}

		[Then(@"the new task should be indented (\d+) place(?:s)?")]
		public void ThenTheNewTaskShouldBeIndented1Place(int indent)
		{
			_context.NewestTask.IndentLevel.Should().Be(indent);
		}

		[Then(@"the new task should be selected")]
		public void ThenTheNewTaskShouldBeSelected()
		{
			_context.NewestTask.Should().Be(_context.TaskBoardViewModel.SelectedTask);
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
			_context.NewestTask.Task.Title.Should().Be(String.Empty);
		}

		[Then(@"the new task should be in edit mode")]
		public void ThenTheNewTaskShouldBeInEditMode()
		{
			_context.NewestTask.IsEditing.Should().BeTrue();

		}

		[Then(@"the task should not be started")]
		public void ThenTheTaskShouldNotBeStarted()
		{
			_context.NewestTask.Task.State.Should().Be(TaskState.NotStarted);
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
