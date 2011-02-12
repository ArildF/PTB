using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
		private IDatabaseServices _databaseServices;

		public SessionFactoryProvider(IDatabaseServices databaseServices)
		{
			_databaseServices = databaseServices;
		}

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

		public ISessionFactory CreateSessionFactory(string path = "MyData.sdf", bool createSchema = false)
		{
			path = Path.GetFullPath(path);

			string configurationFile = Path.Combine(Path.GetDirectoryName(path), 
				Path.GetFileNameWithoutExtension(path) + ".nhconfiguration");
			var serializer = new BinaryFormatter();

			if (!createSchema && File.Exists(configurationFile))
			{
				var configuration = (Configuration) serializer.Deserialize(File.OpenRead(configurationFile));

				return Fluently.Configure(configuration).BuildSessionFactory();
			}
			string connString = String.Format("Data Source={0};Persist Security Info=False", path);

			if (createSchema)
			{
				_databaseServices.CreateDatabaseFile(connString);
			}

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

					serializer.Serialize(File.OpenWrite(configurationFile), config);
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