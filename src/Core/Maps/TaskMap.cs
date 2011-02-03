using FluentNHibernate.Mapping;

namespace Rogue.Ptb.Core.Maps
{
	public sealed class TaskMap : ClassMap<Task>
	{
		public TaskMap()
		{
			Id(t => t.Id);
			Map(t => t.Title);
		}
	}
}
