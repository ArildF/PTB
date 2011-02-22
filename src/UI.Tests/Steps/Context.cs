using System;
using System.Collections.Generic;
using System.Windows.Input;
using Castle.DynamicProxy;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Moq;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.ViewModels;
using StructureMap;
using IInterceptor = Castle.DynamicProxy.IInterceptor;
using NHibernate.Linq;

namespace Rogue.Ptb.UI.Tests.Steps
{
	public class Context : IDisposable
	{
		private readonly Container _container;
		private readonly MockFactory _factory;
		private readonly Provider _provider;
		private readonly Mock<IDialogDisplayer> _dialogDisplayer;

		public Context()
		{
			var bootStrapper = new Bootstrapper();
			bootStrapper.Bootstrap();

			_factory = new MockFactory(MockBehavior.Loose);

			_dialogDisplayer = _factory.Create<IDialogDisplayer>();



			_container = bootStrapper.Container;
			_provider = _container.GetInstance<Provider>();
			_container.Inject<ISessionFactoryProvider>(_provider);
			_container.Inject<ISession>(_provider.GetSession());
			_container.Inject<IDialogDisplayer>(_dialogDisplayer.Object);

			var settings = new UI.Properties.Settings {LastRecentlyUsedTaskboards = null};
			settings.Providers.Clear();
			_container.Inject(settings);

			var startables = _container.Model.GetAllPossible<IStartable>();
			TypeHelperExtensionMethods.ForEach(startables, startup => startup.Start());

			TaskBoardViewModel = Get<TaskBoardViewModel>();
			ToolbarViewModel = Get<ToolbarViewModel>();
		}

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

		public T Get<T>()
		{
			return _container.GetInstance<T>();
		}

		public class Provider : SessionFactoryProvider, ISessionFactoryProvider, IDisposable
		{
			private ISessionFactory _sessionFactory;
			private ISession _session;

			public Provider(IDatabaseServices services, IEnumerable<IDatabaseInitializer> initializers) 
				: base(services, initializers)
			{
				CreatedDatabases = new List<string>();
				OpenedDatabases = new List<string>();
			}

			public override ISession GetSession()
			{
				if (_session == null)
				{
					CreateNewDatabase(String.Empty);
				}
				return _session;
			}

			public override ISessionFactory GetSessionFactory()
			{
				return _sessionFactory;
			}

			public IList<string> CreatedDatabases { get; private set; }

			public IList<string> OpenedDatabases { get; private set; }

			public override  ISessionFactory CreateSessionFactory(string path = "MyData.sdf", bool createSchema = false)
			{
				var config = Fluently.Configure()
					.Database(SQLiteConfiguration.Standard
						.ConnectionString("Data Source=:memory:;Version=3;New=True; Pooling=True; Max Pool Size=1")
						.Raw("connection.release_mode", "on_close"))
					.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<SessionFactoryProvider>())
					.BuildConfiguration();

				var factory = config.BuildSessionFactory();

				var session = factory.OpenSession();

				var generator = new ProxyGenerator();
				_session = generator.CreateInterfaceProxyWithTargetInterface(session, new CloseInterceptor());

				new SchemaExport(config).Execute(false, true, false, session.Connection, Console.Out);

				return _sessionFactory = factory;
			}

			private class CloseInterceptor : IInterceptor
			{
				public void Intercept(IInvocation invocation)
				{
					if (FluentNHibernate.Utils.Extensions.In(invocation.Method.Name, "Close", "Dispose"))
					{
						return;
					}
					invocation.Proceed();
				}
			}

			public override void CreateNewDatabase(string path)
			{
				CreatedDatabases.Add(path);

				base.CreateNewDatabase(path);
			}

			public override void OpenDatabase(string file)
			{
				OpenedDatabases.Add(file);
				if (_sessionFactory == null)
				{
					base.OpenDatabase(file);
				}
			}

			public void Dispose()
			{
				_sessionFactory.Close();
			}

			public void ClearDatabase()
			{
				var session = GetSession();

				foreach (var task in session.Query<Task>())
				{
					session.Delete(task);
				}
			}
		}

		public void Publish<T>(T message)
		{
			Get<IEventAggregator>().Publish(message);
		}

		public void Dispose()
		{
			_provider.Dispose();
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

		public void ClearDatabase()
		{
			_provider.ClearDatabase();
		}
	}
}
