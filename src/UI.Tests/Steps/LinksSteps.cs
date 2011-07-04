using Rogue.Ptb.UI.Behaviors;
using Rogue.Ptb.UI.ViewModels;
using TechTalk.SpecFlow;
using System.Linq;
using FluentAssertions;

namespace Rogue.Ptb.UI.Tests.Steps
{
	[Binding]
	public class LinksSteps
	{
		private readonly Context _context;

		public LinksSteps(Context context)
		{
			_context = context;
		}

		[When(@"I set task ""(.*)"" to be more important than task ""(.*)""")]
		public void WhenISetTaskThreeToBeMoreImportantThanTaskOne(string firstTaskName, string secondTaskName)
		{
			var firstTask = _context.FindTaskVM(firstTaskName);
			var secondTask = _context.FindTaskVM(secondTaskName);

			_context.TaskBoardViewModel.DragCommand.Execute(
				new DragCommandArgs(firstTask, secondTask));
		}

		[Then(@"task ""(.*)"" should be more important than task ""(.*)""")]
		public void ThenTaskOneShouldBeMoreImportantThanTaskThree(TaskViewModel vm1, TaskViewModel vm2)
		{
			vm1.Task.LessImportantTasks.Should().Contain(vm2.Task);
			vm2.Task.MoreImportantTasks.Should().Contain(vm1.Task);
		}

		[Then(@"task ""(.*)"" should be less important than task ""(.*)""")]
		public void ThenTaskOneShouldBeLessImportantThanTaskThree(TaskViewModel vm1, TaskViewModel vm2)
		{
			vm1.Task.MoreImportantTasks.Should().Contain(vm2.Task);
			vm2.Task.LessImportantTasks.Should().Contain(vm1.Task);
		}

		[Then(@"task ""(.*)"" should not be more important than task ""(.*)""")]
		public void ThenTaskThreeShouldNotBeMoreImportantThanTaskOne(TaskViewModel vm1, TaskViewModel vm2)
		{
			vm1.Task.LessImportantTasks.Should().NotContain(vm2.Task);
			vm2.Task.MoreImportantTasks.Should().NotContain(vm1.Task);
		}

		[Then(@"task ""(.*)"" should have (\d+) link to task ""(.*)""")]
		public void ThenTaskThreeShouldHave1LinkToTaskOne(TaskViewModel vm1, int numLinks, TaskViewModel vm2)
		{
			vm1.Task.LessImportantTasks.Concat(vm1.Task.MoreImportantTasks).Count(task => task == vm2.Task)
				.Should().Be(numLinks);
		}



		[StepArgumentTransformation("(.*)")]
		public TaskViewModel StringToTask(string taskName)
		{
			return _context.FindTaskVM(taskName);
		}

	}
}
