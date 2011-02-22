using NHibernate;

namespace Rogue.Ptb.Core.Initializers
{
	public class SetDatabaseVersion : IDatabaseInitializer
	{
		public const int CurrentVersion = 2;

		public void Run(ISession session)
		{
			var version = new DatabaseVersion {Number = CurrentVersion};
			session.Save(version);
		}
	}
}
