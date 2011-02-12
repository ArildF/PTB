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

namespace Rogue.Ptb.UI.Tests.Steps
{
	public class Context : IDisposable
	{
		private readonly Container _container;
		private readonly MockFactory _factory;
		private readonly Provider _provider;
		private Mock<IDialogDisplayer> _dialogDisplayer;

		public Context()
		{
			var bootStrapper = new Bootstrapper();
			bootStrapper.Bootstrap();

			_factory = new MockFactory(MockBehavior.Loose);

			_dialogDisplayer = _factory.Create<IDialogDisplayer>();



			_container = bootStrapper.Container;
			_provider = new Provider();
			_container.Inject<ISessionFactoryProvider>(_provider);
			_container.Inject<ISession>(_provider.Session);
			_container.Inject<IDialogDisplayer>(_dialogDisplayer.Object);

			TaskBoardViewModel = Get<TaskBoardViewModel>();


		}

		public TaskBoardViewModel TaskBoardViewModel { get; private set; }

		public IEnumerable<string> CreatedDatabases
		{
			get { return _provider.CreatedDatabases; }
		}

		public T Get<T>()
		{
			return _container.GetInstance<T>();
		}

		public class Provider : ISessionFactoryProvider, IDisposable
		{
			private ISessionFactory _sessionFactory;
			private ISession _session;

			public Provider()
			{
				CreatedDatabases = new List<string>();
			}
			public ISession Session
			{
				get
				{
					if (_session == null)
					{
						GetSessionFactory();
					}
					return _session;
				}
			}

			public IList<string> CreatedDatabases { get; private set; }

			public ISessionFactory GetSessionFactory()
			{
				return _sessionFactory ?? (_sessionFactory = CreateSessionFactory());
			}

			private ISessionFactory CreateSessionFactory()
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

				return factory;
			}

			private class CloseInterceptor : IInterceptor
			{
				public void Intercept(IInvocation invocation)
				{
					if (invocation.Method.Name.In("Close", "Dispose"))
					{
						return;
					}
					invocation.Proceed();
				}
			}

			public void CreateNewDatabase(string path)
			{
				CreatedDatabases.Add(path);
			}

			public void OpenDatabase(string file)
			{
			}

			public void Dispose()
			{
				_sessionFactory.Close();
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
	}
}
