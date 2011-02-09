using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Moq;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.ViewModels;
using StructureMap;

namespace Rogue.Ptb.UI.Tests.Steps
{
	public class Context : IDisposable
	{
		private readonly Container _container;
		private MockFactory _factory;
		private readonly Provider _provider;

		public Context()
		{
			var bootStrapper = new Bootstrapper();
			bootStrapper.Bootstrap();

			_factory = new MockFactory(MockBehavior.Loose);


			_container = bootStrapper.Container;
			_provider = new Provider();
			_container.Inject<ISessionFactoryProvider>(_provider);

			TaskBoardViewModel = Get<TaskBoardViewModel>();


		}

		public TaskBoardViewModel TaskBoardViewModel { get; private set; }

		public T Get<T>()
		{
			return _container.GetInstance<T>();
		}

		public class Provider : ISessionFactoryProvider, IDisposable
		{
			private ISessionFactory _sessionFactory;
			

			public ISessionFactory GetSessionFactory()
			{
				return _sessionFactory ?? (_sessionFactory = CreateSessionFactory());
			}

			private static ISessionFactory CreateSessionFactory()
			{
				var config = Fluently.Configure()
					.Database(SQLiteConfiguration.Standard
						.ConnectionString("Data Source=:memory:;Version=3;New=True; Pooling=True; Max Pool Size=1"))
					.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<SessionFactoryProvider>())
					.BuildConfiguration();

				var factory = config.BuildSessionFactory();


				using (var session = factory.OpenSession())
					new SchemaExport(config).Execute(false, true, false, session.Connection, Console.Out);

				return factory;
			}

			public void CreateNewDatabase(string path)
			{
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
	}
}
