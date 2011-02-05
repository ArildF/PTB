using System;
using System.Windows.Input;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Rogue.Ptb.UI.Registration
{
	internal class CommandRegistrationConvention : IRegistrationConvention
	{
		public void Process(Type type, Registry registry)
		{
			if (type.IsAbstract || !type.IsClass || !typeof(ICommand).IsAssignableFrom(type))
			{
				return;
			}

			registry.AddType(typeof(ICommand), type, type.Name);
		}
	}
}