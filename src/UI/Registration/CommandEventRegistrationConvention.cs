using System;
using System.Windows.Input;
using Rogue.Ptb.UI.Commands;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

namespace Rogue.Ptb.UI.Registration
{
	public class CommandEventRegistrationConvention : IRegistrationConvention
	{
		public void ScanTypes(TypeSet types, Registry registry)
		{
			foreach (var type in types.AllTypes())
			{
				if (!type.IsAbstract && typeof(ICommandEvent).IsAssignableFrom(type))
				{
					registry.ForSingletonOf<ICommand>().Configure(pg => pg.AddType(typeof(EventCommand<>).MakeGenericType(type), type.Name));
				}
			}
		}
	}
}
