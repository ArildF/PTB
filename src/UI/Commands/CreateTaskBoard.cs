using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.Commands
{
	public class CreateTaskBoard : NoParameterCommandBase
	{
		private readonly ISessionFactoryProvider _sessionFactoryProvider;

		public CreateTaskBoard(ISessionFactoryProvider sessionFactoryProvider)
		{
			_sessionFactoryProvider = sessionFactoryProvider;
		}


		protected override void Execute()
		{
			_sessionFactoryProvider.CreateNewDatabase("Board.taskboard");
			
		}
	}
}
