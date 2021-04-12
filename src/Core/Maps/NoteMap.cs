using FluentNHibernate.Mapping;

namespace Rogue.Ptb.Core.Maps
{
	public class NoteMap : ClassMap<Note>
	{
		public NoteMap()
		{
			Id(t => t.Id).GeneratedBy.Guid();
			Map(n => n.Created);
			Map(n => n.Markdown);
			Map(n => n.Modified);
		}
	}
}