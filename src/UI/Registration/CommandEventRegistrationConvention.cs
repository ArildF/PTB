using System;
using System.Windows.Input;
using Rogue.Ptb.UI.Commands;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Rogue.Ptb.UI.Registration
{
	public class CommandEventRegistrationConvention : IRegistrationConvention
	{
		public void Process(Type type, Registry registry)
		{
			if (!type.IsAbstract && typeof(ICommandEvent).IsAssignableFrom(type))
			{
				registry.AddType(typeof(ICommand), typeof(EventCommand<>).MakeGenericType(type), type.Name);
			}
		}

	}
}
