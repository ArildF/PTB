using NHibernate;

namespace Rogue.Ptb.Core
{
	public interface ISessionFactoryProvider
	{
		ISessionFactory GetSessionFactory();
		void CreateNewDatabase(string path);
	}
}
