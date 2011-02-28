using System.Windows.Input;
using Rogue.Ptb.UI.Commands;

namespace Rogue.Ptb.UI.ViewModels
{
	public class ShellViewModel : IShellViewModel, ICommandResolver
	{
		private readonly ICommandResolver _resolver;

		public ShellViewModel(ICommandResolver resolver)
		{
			_resolver = resolver;
		}

		public ICommand Resolve(CommandName commandName)
		{
			return _resolver.Resolve(commandName);
		}
	}

}
