using FluentNHibernate.Mapping;

namespace Rogue.Ptb.Core.Maps
{
	public sealed class TaskMap : ClassMap<Task>
	{
		public TaskMap()
		{
			Id(t => t.Id).GeneratedBy.Assigned();
			Map(t => t.Title).Not.Nullable();
			Map(t => t.State).CustomType<int>().Not.Nullable();
			Map(t => t.CreatedDate).Not.Nullable();
			Map(t => t.ModifiedDate).Not.Nullable();
			Map(t => t.StartedDate);
			Map(t => t.CompletedDate);
			Map(t => t.AbandonedDate);
			Map(t => t.StateChangedDate);
			HasMany(t => t.Links).KeyColumn("Task_id").AsSet().Cascade.All().Fetch.Join();
		}
	}
}
