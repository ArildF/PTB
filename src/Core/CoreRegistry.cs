using NHibernate;
using ReactiveUI;
using Rogue.Ptb.Core.Repositories;
using StructureMap.Configuration.DSL;

namespace Rogue.Ptb.Core
{
	public class CoreRegistry : Registry
	{
		public CoreRegistry()
		{
			Scan(scanner =>
				{
					scanner.WithDefaultConventions();
					scanner.TheCallingAssembly();
				});

			For(typeof (IRepository<>)).Use(typeof (RepositoryBase<>));

			For<ISession>().Use(c => c.GetInstance<ISessionFactoryProvider>().GetSessionFactory().OpenSession());
			ForSingletonOf<ISessionFactoryProvider>().Use<SessionFactoryProvider>();
			ForSingletonOf<IEventAggregator>().Use<EventAggregator>();
		}
	}
}
