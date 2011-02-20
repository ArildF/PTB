using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Rogue.Ptb.Core;
using Rogue.Ptb.Core.Export;
using Rogue.Ptb.Infrastructure;
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
		private TaskDto[] _exportedDtos;

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

		[Given(@"an export file containing these tasks at ""(.*)""")]
		public void GivenAnExportFileContainingTheseTasksAtCFooExport_Xml(string path, Table table)
		{
			path = TestifyPath(path);

			Directory.CreateDirectory(Path.GetDirectoryName(path));


			var tasks = table.Rows.Select(r => new Task {Title = r["Title"]});
			var repos = _context.Get<IRepository<Task>>();
			tasks.ForEach(repos.Save);

			var exporter = _context.Get<ITasksExporter>();
			exporter.ExportAll(path);

			_context.ClearDatabase();
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

		[Given(@"that I enter ""(.*)"" in the export taskboard dialog")]
		public void GivenThatIEnterCFooBar_TaskboardInTheExportTaskboardDialog(string path)
		{
			string exportPath = TestifyPath(path);
			Console.WriteLine(exportPath);
			_context.SetUpDialogResult(new ExportTaskBoardDialogResult(exportPath));

			Directory.CreateDirectory(Path.GetDirectoryName(exportPath));
		}
		[Given(@"that I enter ""(.*)"" in the import taskboard dialog")]
		public void WhenThatIEnterCFooBarTaskboardImport_TaskboardInTheImportTaskboardDialog(string path)
		{
			path = TestifyPath(path);

			_context.SetUpDialogResult(new ImportTaskBoardDialogResult(path));

		}

		[When(@"I click import tasks")]
		public void WhenIClickImportTasks()
		{
			_context.GetCommand<ImportTaskBoard>().Execute(null);
		}


		[Given(@"that everything is saved")]
		public void GivenThatEverythingIsSaved()
		{
			_context.Publish(new SaveAllTasks());
		}


		[When(@"I create a new taskboard")]
		[Given(@"I create a new taskboard")]
		public void WhenICreateANewTaskboard()
		{
			_context.GetCommand<CreateTaskBoard>().Execute(null);
		}

		[When(@"I open a taskboard")]
		public void WhenIOpenATaskboard()
		{
			_context.GetCommand<OpenTaskBoard>().Execute(null);
		}

		[When(@"I click export task")]
		public void WhenIClickExportTask()
		{
			_context.GetCommand<ExportTaskBoard>().Execute(null);
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
			_context.ToolbarViewModel.LastRecentlyUsedTaskboards.Select(menuItem => menuItem.Text).Should().ContainInOrder(paths);
		}

		[Then(@"the dropdown for the open button should contain no items")]
		public void ThenTheDropdownForTheOpenButtonShouldContainNoItems()
		{
			_context.ToolbarViewModel.LastRecentlyUsedTaskboards.Should().BeEmpty();
		}

		[Then(@"the tasks should be exported to a ""(.*)""")]
		public void ThenTheTasksShouldBeExportedToAcFooBarTaskboard(string path)
		{
			path = TestifyPath(path);

			var serializer = new XmlSerializer(typeof (TaskDto[]));
			using (var stream = File.OpenRead(path))
			{
				_exportedDtos = (TaskDto[])serializer.Deserialize(stream);
				var mappings = new DtoMapper();
			}
		}

		[Then(@"the exported tasks should contain (\d+) tasks")]
		public void ThenTheExportedTasksShouldContain3Tasks(int num)
		{
			_exportedDtos.Length.Should().Be(num);
		}

		


		[Then(@"task \#(\d+) in the exported tasks should have the title ""(.*)""")]
		public void ThenTask2InTheExportedTasksShouldHaveTheTitleBar(int ordinal, string expectedTitle)
		{
			_exportedDtos[ordinal - 1].Title.Should().BeEquivalentTo(expectedTitle);
		}

		[Then(@"the exported tasks should not have empty IDs")]
		public void ThenTheTasksShouldNotHaveEmptyIDs()
		{
			_exportedDtos.Should().OnlyContain(t => t.Id != Guid.Empty);
		}

		[Then(@"the loaded tasks should not have empty IDs")]
		public void ThenTheLoadedTasksShouldNotHaveEmptyIDs()
		{
			_context.TaskBoardViewModel.Tasks.Should().OnlyContain(t => t.Task.Id != Guid.Empty);
		}


		private static string TestifyPath(string path)
		{
			return Path.Combine(
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
				path.Replace(":", ""));
		}
	}
}
