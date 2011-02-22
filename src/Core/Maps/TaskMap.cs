using FluentNHibernate.Mapping;

namespace Rogue.Ptb.Core.Maps
{
	public sealed class TaskMap : ClassMap<Task>
	{
		public TaskMap()
		{
			Id(t => t.Id);
			Map(t => t.Title).Not.Nullable();
			Map(t => t.State).CustomType<int>().Not.Nullable();
			Map(t => t.CreatedDate).Not.Nullable();
		}
	}
}
