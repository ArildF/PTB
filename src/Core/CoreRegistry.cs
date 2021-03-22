using AutoMapper;
using NHibernate;
using Rogue.Ptb.Core.Repositories;
using Rogue.Ptb.Core.SqlLite;
using StructureMap;

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
					scanner.AddAllTypesOf<IDatabaseInitializer>();

					scanner.AddAllTypesOf<Profile>();
				});


			For<ISession>().Use(c => c.GetInstance<ISessionFactoryProvider>().GetSessionFactory().OpenSession());
			ForSingletonOf<ISessionFactoryProvider>().Use<SessionFactoryProvider>();
			For(typeof (IRepository<>)).Use(typeof (RepositoryBase<>));
			For<IDatabaseServices>().Use<SqlLiteDatabaseServices>();

			For<IConfigurationProvider>().Use(c => CreateProvider(c)).Singleton();
			For<IMapper>().Use(c => new Mapper(c.GetInstance<IConfigurationProvider>(), c.GetInstance));
		}
		
		IConfigurationProvider CreateProvider(IContext c) => new MapperConfiguration(configuration =>
		{
			foreach (var profile in c.GetAllInstances<Profile>())
			{
				configuration.AddProfile(profile);
			}
		});
	}
}
