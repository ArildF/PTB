using Rogue.Ptb.UI.Commands;
using Rogue.Ptb.UI.Registration;
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
					scanner.With(new CommandEventRegistrationConvention());
				});

			For<ICommandResolver>().Use<CommandResolver>();
		}
	}

}