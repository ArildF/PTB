using FluentNHibernate.Mapping;

namespace Rogue.Ptb.Core.Maps
{
	public sealed class LinkMap : ClassMap<Link>
	{
		public LinkMap()
		{
			Id(link => link.Id).GeneratedBy.Guid();
			References(link => link.LinkTo);
			Map(link => link.Type);
		}
	}
}
