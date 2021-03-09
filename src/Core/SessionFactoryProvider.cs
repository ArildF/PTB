using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Util;
using Rogue.Ptb.Core.Initializers;

namespace Rogue.Ptb.Core
{
	public class SessionFactoryProvider : ISessionFactoryProvider
	{
		private ISessionFactory _factory;
		private readonly IDatabaseServices _databaseServices;
		private readonly IEnumerable<IDatabaseInitializer> _initializers;

		public SessionFactoryProvider(IDatabaseServices databaseServices, IEnumerable<IDatabaseInitializer> initializers)
		{
			_databaseServices = databaseServices;
			_initializers = initializers;
		}

		public virtual ISessionFactory GetSessionFactory()
		{
			return _factory ??= CreateSessionFactory();
		}

		public virtual void CreateNewDatabase(string path)
		{
			OpenDatabase(path, true);

			using (var session = GetSession())
			{
				_initializers.ForEach(i => i.Run(session));

				session.Flush();
			}
		}

		public virtual void OpenDatabase(string file)
		{
			OpenDatabase(file, false);

			using (var session = GetSession())
			{
				var version = session.Query<DatabaseVersion>().FirstOrDefault();

				if (version == null || version.Number != SetDatabaseVersion.CurrentVersion)
				{
					throw new InvalidOperationException(
						"Wrong database version. Expected version " + SetDatabaseVersion.CurrentVersion);
				}
			}

		}

		private void OpenDatabase(string path, bool createSchema)
		{
			if (_factory != null)
			{
				_factory.Close();
			}

			_factory = CreateSessionFactory(path, createSchema);
		}

		public virtual ISessionFactory CreateSessionFactory(string path = "MyData.sdf", bool createSchema = false)
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
				.Database(SQLiteConfiguration.Standard
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

		public virtual ISession GetSession()
		{
			return GetSessionFactory().OpenSession();
		}
	}
}