using System;
using Rogue.Ptb.UI.Commands;
using Rogue.Ptb.UI.Properties;
using Rogue.Ptb.UI.Registration;
using Rogue.Ptb.UI.Views;
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
			ForSingletonOf<IShellView>().Use<ShellView>();
			For<ICommandResolver>().Use<CommandResolver>();
			ForSingletonOf<Properties.Settings>().Use(c => Properties.Settings.Default);

		}
	}

}