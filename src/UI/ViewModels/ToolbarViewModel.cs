using System.Windows.Input;
using Rogue.Ptb.UI.Commands;

namespace Rogue.Ptb.UI.ViewModels
{
	public class ToolbarViewModel : IToolbarViewModel, ICommandResolver
	{
		private readonly ICommandResolver _resolver;

		public ToolbarViewModel(ICommandResolver resolver)
		{
			_resolver = resolver;
		}

		public ICommand Resolve(CommandName commandName)
		{
			return _resolver.Resolve(commandName);
		}
	}

}
