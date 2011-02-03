using Rogue.Ptb.UI.Commands;
using StructureMap.Configuration.DSL;

namespace Rogue.Ptb.UI
{
	internal class UIRegistry : Registry
	{
		public UIRegistry()
		{
			Scan(scanner =>
				{
					scanner.TheCallingAssembly();
					scanner.WithDefaultConventions();

					scanner.With(new CommandRegistrationConvention());
				});

			For<ICommandResolver>().Use<CommandResolver>();
		}
	}

}