using StructureMap.Configuration.DSL;

namespace Rogue.Ptb.Infrastructure
{
	public class InfrastructureRegistry : Registry
	{
		public InfrastructureRegistry()
		{
			Scan(scanner =>
				{
					scanner.TheCallingAssembly();
					scanner.WithDefaultConventions();
				});
			ForSingletonOf<IEventAggregator>().Use<EventAggregator>();
		}
	}
}
