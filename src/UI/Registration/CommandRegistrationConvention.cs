using System.Windows.Input;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

namespace Rogue.Ptb.UI.Registration
{
	internal class CommandRegistrationConvention : IRegistrationConvention
	{
		public void ScanTypes(TypeSet types, Registry registry)
		{
			foreach (var type in types.AllTypes())
			{
				if (type.IsAbstract || !type.IsClass || !typeof(ICommand).IsAssignableFrom(type))
				{
					continue;
				}

				registry.AddType(typeof(ICommand), type, type.Name);
			}
		}
	}
}