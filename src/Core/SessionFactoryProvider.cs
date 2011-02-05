using System;
using System.Diagnostics;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Rogue.Ptb.Core
{
	public class SessionFactoryProvider : ISessionFactoryProvider
	{
		private ISessionFactory _factory;

		public ISessionFactory GetSessionFactory()
		{
			return _factory ?? (_factory = CreateSessionFactory());
		}

		public void CreateNewDatabase(string path)
		{
			OpenDatabase(path, true);
		}

		public void OpenDatabase(string file)
		{
			OpenDatabase(file, false);
		}

		private void OpenDatabase(string path, bool createSchema)
		{
			if (_factory != null)
			{
				_factory.Close();
			}

			_factory = CreateSessionFactory(path, createSchema);
		}

		public static ISessionFactory CreateSessionFactory(string path = "MyData.sdf", bool createSchema = false)
		{
			string connString = String.Format("Data Source={0};Persist Security Info=False", path);

			var factory = Fluently.Configure()
				.Database(MsSqlCeConfiguration.Standard
							.ConnectionString(c => c.Is(connString)))
				.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<SessionFactoryProvider>())
				.ExposeConfiguration(config =>
				{
					if (createSchema)
					{
						BuildSchema(config);
					}
				})
				//.Diagnostics(dc => dc.Enable().OutputToFile("Diagnostics.txt"))
				.BuildSessionFactory();

			return factory;
		}

		private static void BuildSchema(Configuration obj)
		{
			new SchemaExport(obj).Create(s => Debug.WriteLine(s), true);
		}
	}
}