using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Serialization;
using AutoMapper;
using NHibernate.Linq;
using Rogue.Ptb.Core;
using Rogue.Ptb.Core.Export;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Commands;
using TechTalk.SpecFlow;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NHibernate.Util;

namespace Rogue.Ptb.UI.Tests.Steps
{
	[Binding]
	public class TaskBoardSteps : TechTalk.SpecFlow.Steps
	{
		private readonly Context _context;
		private bool _databaseChangedMessageReceived;
		private TaskDto[] _exportedDtos;
		private Task[] _loadedTasks;

		private Random _random = new Random();

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

		[Given(@"a varied set of tasks loaded into the taskboard")]
		public void GivenAVariedSetOfTasksLoadedIntoTheTaskboard()
		{
			GivenThatIHaveCreatedANewDatabase();
			var repos = _context.Get<IRepository<Task>>();

			var tasks = Enumerable.Range(0, 100).Select(_ => CreateRandomTask()).ToArray();

			repos.SaveAll(tasks);

			_loadedTasks = tasks;

			_context.Publish(new DatabaseChanged("whatever"));

		}

		private Task CreateRandomTask()
		{
			DateTimeHelper.MoveAheadBy(TimeSpan.FromMinutes(_random.Next(50)));

			var task = new Task {Title = Guid.NewGuid().ToString()};

			DateTimeHelper.MoveAheadBy(TimeSpan.FromMinutes(_random.Next(50)));

			if (DateTime.Now.Ticks % 2 == 0)
			{
				task.Start();
			}

			DateTimeHelper.MoveAheadBy(TimeSpan.FromMinutes(_random.Next(50)));

			if (DateTime.Now.Ticks % 3 == 0)
			{
				task.Complete();
			}

			DateTimeHelper.MoveAheadBy(TimeSpan.FromMinutes(_random.Next(50)));

			if (DateTime.Now.Ticks % 4 == 0)
			{
				task.Abandon();
			}

			return task;
		}

		[Given(@"an export file containing these tasks at ""(.*)""")]
		public void GivenAnExportFileContainingTheseTasksAtCFooExport_Xml(string path, Table table)
		{
			path = TestifyPath(path);

			Directory.CreateDirectory(Path.GetDirectoryName(path));


			GivenThatIHaveCreatedANewDatabase();

			var tasks = table.Rows.Select(r => new Task {Title = r["Title"]});
			var repos = _context.Get<IRepository<Task>>();
			tasks.ForEach(repos.InsertNew);

			var exporter = _context.Get<ITasksExporter>();
			exporter.ExportAll(path);
		}

		[Given(@"that I have created a new database")]
		public void GivenThatIHaveCreatedANewDatabase()
		{
			var filename = Guid.NewGuid().ToString();

			_context.Get<ISessionFactoryProvider>().CreateNewDatabase(filename);
		}


		[Given(@"an open taskboard")]
		public void GivenAnOpenTaskboard()
		{
			Given("that I have created a new database");

			_context.Publish(new DatabaseChanged("whatever"));
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

		[Given(@"that the following taskboards already exist:")]
		public void GivenThatTheFollowingTaskboardsAlreadyExist(Table table)
		{
			foreach (var tableRow in table.Rows)
			{
				GivenThatTheDatabaseCFooBar_TaskboardAlreadyExists(tableRow[0]);
			}
		}


		[Given(@"that the database ""(.*)"" already exists")]
		public void GivenThatTheDatabaseCFooBar_TaskboardAlreadyExists(string path)
		{
			_context.Get<ISessionFactoryProvider>().CreateNewDatabase(path);
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

		[When(@"I click Collapse All")]
		public void WhenIClickCollapseAll()
		{
			_context.Publish<CollapseAll>();
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

		[When(@"I reload the taskboard")]
		public void WhenIReloadTheTaskboard()
		{
			_context.Publish(new SaveAllTasks());
			_context.Publish(new ReloadAllTasks());
		}

		[When(@"foo")]
		public void WhenFoo()
		{
			var filename = Path.GetTempFileName();
			_context.Get<ISessionFactoryProvider>().CreateNewDatabase(filename);

			var task = new Task {Title = "Parent task"};
			var childTask = task.CreateSubTask();
			childTask.Title = "Child task";

			using (var repos = _context.Get<ITasksRepository>())
				repos.SaveAll(new[] { task, childTask });

			using (var repos = _context.Get<ITasksRepository>())
			{
				var loadedTasks = repos.FindAll().ToArray();
				Console.WriteLine(loadedTasks);
			}

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
				var mappings = new DtoMapper(_context.Get<IMapper>());
			}
		}

		[Then(@"the exported tasks should contain (\d+) tasks")]
		public void ThenTheExportedTasksShouldContain3Tasks(int num)
		{
			_exportedDtos.Length.Should().Be(num);
		}

		[Then(@"the (visible )?tasks should be in this order:")]
		public void ThenTheTasksShouldBeInThisOrder(string visible, Table table)
		{
			var vms = _context.TaskBoardViewModel.Tasks.Select(t =>  t);
			
			if (visible == "visible ")
			{
				vms = vms.Where(vm => vm.IsVisible);
			}

			vms.Count().Should().Be(table.RowCount);

			var tasks = vms.Select(t => t.Title).Zip(
				table.Rows.Select(r => r[0]), Tuple.Create);

			foreach (var tuple in tasks)
			{
				tuple.Item1.Should().Be(tuple.Item2);
			}

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

		[Then(@"the loaded tasks should have the same attributes as the original set of tasks")]
		public void ThenTheLoadedTasksShouldHaveTheSameAttributesAsTheOriginalSetOfTasks()
		{
			var newTasks = _context.Get<IRepository<Task>>().FindAll().ToArray();
			foreach (var newTask in newTasks)
			{
				Task task = newTask;
				var oldTask = _loadedTasks.FirstOrDefault(t => t.Id == task.Id);
				oldTask.Should().NotBeNull();

				var dateFuncs = new Expression<Func<Task, object>>[]
					{
						t => t.CreatedDate,
						t => t.AbandonedDate,
						t => t.CompletedDate,
						t => t.ModifiedDate,
						t => t.StartedDate,
						t => t.StateChangedDate
					};

				var links = new Expression<Func<Task, object>>[]
					{
						t => t.LessImportantTasks,
						t => t.MoreImportantTasks,
						t => t.Parent,
						t => t.SubTasks
					};

				CheckDates(oldTask, newTask, dateFuncs);

				var options = new EquivalencyAssertionOptions<Task>().IncludingAllDeclaredProperties();

				foreach (var expression in dateFuncs.Skip(1).Concat(links))
				{
					options = options.Excluding(expression);
				}

				newTask.Should().BeEquivalentTo(oldTask, _ => options);
			}
		}

		private static void CheckDates(Task oldTask, Task newTask, IEnumerable<Expression<Func<Task, object>>> dateFuncs)
		{
			foreach (var expression in dateFuncs)
			{
				var func = expression.Compile();
				var oldDate = (DateTime?) func(oldTask);
				var newDate = (DateTime?) func(newTask);

				if (oldDate == null)
				{
					newDate.Should().Be(null);
				}
				else
				{
					oldDate.Should().BeWithin(TimeSpan.FromSeconds(0.5)).Before(newDate.GetValueOrDefault());
				}
			}
		}


		private static string TestifyPath(string path)
		{
			return Path.Combine(
				Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
				path.Replace(":", ""));
		}
	}
}
