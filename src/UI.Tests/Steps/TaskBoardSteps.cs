using System.Linq;
using Rogue.Ptb.Core;
using Rogue.Ptb.UI.Commands;
using TechTalk.SpecFlow;
using FluentAssertions;

namespace Rogue.Ptb.UI.Tests.Steps
{
	[Binding]
	public class TaskBoardSteps
	{
		private readonly Context _context;
		private bool _databaseChangedMessageReceived;

		public TaskBoardSteps(Context context)
		{
			_context = context;

			_context.Subscribe<DatabaseChanged>(dbc => _databaseChangedMessageReceived = true);
		}

		[Given(@"that I enter ""(.*)"" in the create taskboard dialog")]
		public void GivenThatIEnterCFooBar_TaskboardInTheCreateTaskboardDialog(string path)
		{
			_context.SetUpDialogResult(new CreateTaskBoardDialogResult(path));

		}

		[Given(@"that I enter ""(.*)"" in the open taskboard dialog")]
		public void GivenThatIEnterCFooBar_TaskboardInTheOpenTaskboardDialog(string path)
		{
			_context.SetUpDialogResult(new OpenTaskBoardDialogResult(path));
		}

		[Given(@"that I open ""(.*)""")]
		public void GivenThatIOpenCBarbarFoofoo_Taskboard(string path)
		{
			GivenThatIEnterCFooBar_TaskboardInTheOpenTaskboardDialog(path);
			WhenIOpenATaskboard();
		}

		[When(@"I create a new taskboard")]
		public void WhenICreateANewTaskboard()
		{
			_context.GetCommand<CreateTaskBoard>().Execute(null);
		}

		[When(@"I open a taskboard")]
		public void WhenIOpenATaskboard()
		{
			_context.GetCommand<OpenTaskBoard>().Execute(null);
		}

		[Then(@"a new taskboard database should be created in ""(.*)""")]
		public void ThenANewTaskboardDatabaseShouldBeCreatedInCFooBar_Taskboard(string path)
		{
			_context.CreatedDatabases.Last().Should().BeEquivalentTo(path);
		}
		
		[Then(@"a taskboard should be loaded from ""(.*)""")]
		public void ThenATaskboardShouldBeLoadedFromCFooBar_Taskboard(string path)
		{
			_context.OpenedDatabases.Last().Should().BeEquivalentTo(path);
		}

		[Then(@"a new taskboard should be loaded")]
		public void ThenANewTaskboardShouldBeLoaded()
		{
			_databaseChangedMessageReceived.Should().BeTrue();
		}

		[Then(@"the dropdown for the open button should display these in this order:")]
		public void ThenTheDropdownForTheOpenButtonShouldDisplayTheseInThisOrder(Table table)
		{
			var paths = table.Rows.Select(row => row[0]);
			_context.ToolbarViewModel.LastRecentlyUsedTaskboards.Should().ContainInOrder(paths);
		}

		[Then(@"the dropdown for the open button should contain no items")]
		public void ThenTheDropdownForTheOpenButtonShouldContainNoItems()
		{
			_context.ToolbarViewModel.LastRecentlyUsedTaskboards.Should().BeEmpty();
		}
	}
}
