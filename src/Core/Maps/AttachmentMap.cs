using FluentNHibernate.Mapping;

namespace Rogue.Ptb.Core.Maps
{
	public class AttachmentMap : ClassMap<Attachment>
	{
		public AttachmentMap()
		{
			Id(a => a.Id).GeneratedBy.Assigned();
			Map(a => a.Content);
			Map(a => a.ContentType);
			Map(a => a.Name);
			Map(a => a.Created);
		}
	}
}