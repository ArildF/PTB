using FluentNHibernate.Mapping;

namespace Rogue.Ptb.Core.Maps
{
	public sealed class DatabaseVersionMap : ClassMap<DatabaseVersion>
	{
		public DatabaseVersionMap()
		{
			Map(v => v.Number).Not.Nullable();
			Id(v => v.Id);
		}
	}
}
