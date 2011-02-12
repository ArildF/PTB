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
			_context.SetUpDialogResult<CreateTaskBoardDialogResult>(new CreateTaskBoardDialogResult(path));

		}

		[When(@"I create a new taskboard")]
		public void WhenICreateANewTaskboard()
		{
			_context.GetCommand<CreateTaskBoard>().Execute(null);
		}

		[Then(@"a new taskboard database should be created in ""(.*)""")]
		public void ThenANewTaskboardDatabaseShouldBeCreatedInCFooBar_Taskboard(string path)
		{
			_context.CreatedDatabases.Last().Should().BeEquivalentTo(path);
		}

		[Then(@"a new taskboard should be loaded")]
		public void ThenANewTaskboardShouldBeLoaded()
		{
			_databaseChangedMessageReceived.Should().BeTrue();
		}
	}
}
