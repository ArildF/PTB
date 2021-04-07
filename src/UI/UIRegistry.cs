using System;
using Rogue.Ptb.Core;
using Rogue.Ptb.UI.Commands;
using Rogue.Ptb.UI.Registration;
using Rogue.Ptb.UI.Services;
using Rogue.Ptb.UI.Views;
using StructureMap;

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
			ForConcreteType<CommandLineLoader>();
			ForSingletonOf<Properties.Settings>().Use(c => Properties.Settings.Default);
			

		}
	}

}