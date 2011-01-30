using StructureMap.Configuration.DSL;

namespace Rogue.Ptb.Core
{
	public class CoreRegistry : Registry
	{
		public CoreRegistry()
		{
			Scan(scanner =>
				{
					scanner.WithDefaultConventions();
					scanner.TheCallingAssembly();
				});
		}
	}
}
