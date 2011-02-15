using System;
using System.Windows.Input;
using StructureMap;

namespace Rogue.Ptb.UI.Commands
{
	public class CommandResolver : ICommandResolver
	{
		private readonly IContainer _container;

		public CommandResolver(IContainer container)
		{
			_container = container;
		}

		public ICommand Resolve(CommandName commandName)
		{
			return _container.TryGetInstance<ICommand>(commandName.Name);
		}
	}
}
