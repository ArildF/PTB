﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Diagnostics;
using Moq;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.ViewModels;
using StructureMap;

namespace Rogue.Ptb.UI.Tests.Steps
{
	public class Context 
	{
		private readonly Container _container;
		private readonly Provider _provider;
		private readonly Mock<IDialogDisplayer> _dialogDisplayer;

		public Context()
		{
			Options = new Options();
			var bootStrapper = new Bootstrapper();
			bootStrapper.Bootstrap(new string[]{});

			var repository = new MockRepository(MockBehavior.Loose);

			_dialogDisplayer = repository.Create<IDialogDisplayer>();



			_container = bootStrapper.Container;
			_provider = _container.GetInstance<Provider>();
			_container.Inject<ISessionFactoryProvider>(_provider);
			_container.Inject(_dialogDisplayer.Object);
			_container.Inject(Options);

			var settings = new Properties.Settings {LastRecentlyUsedTaskboards = null};
			settings.Providers.Clear();
			_container.Inject(settings);

			var startables = _container.Model.GetAllPossible<IStartable>();
			foreach (var startable in startables)
			{
				startable.Start();
			}

			TaskBoardViewModel = Get<TaskBoardViewModel>();
			ToolbarViewModel = Get<ToolbarViewModel>();
			ShellViewModel = Get<ShellViewModel>();


		
		}

		public ShellViewModel ShellViewModel { get; private set; }

		public TaskBoardViewModel TaskBoardViewModel { get; private set; }

		public IEnumerable<string> CreatedDatabases
		{
			get { return _provider.CreatedDatabases; }
		}

		public IEnumerable<string> OpenedDatabases
		{
			get { return _provider.OpenedDatabases; }
		}

		public ToolbarViewModel ToolbarViewModel
		{
			get; private set;
		}

		public Options Options { get; private set; }

		public T Get<T>()
		{
			return _container.GetInstance<T>();
		}

		public class Provider : SessionFactoryProvider
		{
			private readonly IContainer _container;

			public Provider(IDatabaseServices services, IEnumerable<IDatabaseInitializer> initializers, IContainer container) 
				: base(services, initializers)
			{
				_container = container;
				DatabasePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
				Directory.CreateDirectory(DatabasePath);

				CreatedDatabases = new List<string>();
				OpenedDatabases = new List<string>();
			}

			public string DatabasePath { get; set; }

			public IList<string> CreatedDatabases { get; }

			public IList<string> OpenedDatabases { get; }

			public class SqlStatementInterceptor : EmptyInterceptor
			{
				public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
				{
					Trace.WriteLine(sql.ToString());
					return sql;
				}
			}

			public override  ISessionFactory CreateSessionFactory(string path = "MyData.sdf", bool createSchema = false)
			{
				var filename = Path.GetFileName(path);
				filename = Path.Combine(DatabasePath, filename);

				string connString = String.Format(
					$"Data Source={filename};Version=3;BinaryGuid=False;Persist Security Info=False");
				                                                  

				if (createSchema)
				{
					var services = _container.GetInstance<IDatabaseServices>();
					services.CreateDatabaseFile(connString);
				}

				var config = Fluently.Configure()
					.Database(SQLiteConfiguration.Standard
						.ConnectionString(connString))
					.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<SessionFactoryProvider>())
					.ExposeConfiguration(c => c.SetInterceptor(new SqlStatementInterceptor()))
					.Diagnostics(d => d.Enable(true).RegisterListener(new StringLambdaOutputListener(s => Debug.WriteLine(s))))
					.BuildConfiguration();


				var factory = config.BuildSessionFactory();


				var session = factory.OpenSession();

				if (createSchema)
				{
					
					new SchemaExport(config).Execute(false, true, false, session.Connection, StreamWriter.Null);
				}

				return  factory;
			}


			public override void CreateNewDatabase(string path)
			{
				CreatedDatabases.Add(path);


				base.CreateNewDatabase(path);
			}

			public override void OpenDatabase(string file)
			{
				OpenedDatabases.Add(file);
					base.OpenDatabase(file);
			}

		}

		public void Publish<T>(T message = default(T))
		{
			Get<IEventAggregator>().Publish(message);
		}


		public void SetUpDialogResult<T>(T result) where T : DialogReturnValueBase
		{
			_dialogDisplayer.Setup(displayer => displayer.ShowDialogFor<T>(null)).Returns(result);

		}

		public ICommand GetCommand<T>()
		{
			return (ICommand) Get<T>();
		}

		public void Subscribe<T>(Action<T> handler)
		{
			Get<IEventAggregator>().Listen<T>().Subscribe(handler);
		}

		public TaskViewModel FindTaskVm(string taskName)
		{
			return TaskBoardViewModel.Tasks.First(t => t.Title == taskName);
		}

		public TaskViewModel NewestTask
		{
			get { return TaskBoardViewModel.Tasks.OrderByDescending(t => t.Task.CreatedDate).First(); }
		}


		public TaskViewModel TaskByOrdinal(int num)
		{
			return TaskBoardViewModel.Tasks.Skip(num - 1).First();
		}
	}
}
