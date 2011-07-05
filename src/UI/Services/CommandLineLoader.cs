using System.Windows.Input;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Commands;

namespace Rogue.Ptb.UI.Services
{
	public class CommandLineLoader : IStartable
	{
		private readonly IEventAggregator _eventAggregator;
		private readonly OpenTaskBoard _openTaskBoardCommand;
		private readonly Options _options;

		public CommandLineLoader(IEventAggregator eventAggregator, OpenTaskBoard openTaskBoardCommand, Options options)
		{
			_eventAggregator = eventAggregator;
			_openTaskBoardCommand = openTaskBoardCommand;
			_options = options;
		}

		public void Start()
		{
			_eventAggregator.ListenOnScheduler<ApplicationIdle>(Handler);
		}

		private void Handler(ApplicationIdle applicationIdle)
		{
			if (_options.TaskboardPath != null)
			{
				((ICommand)_openTaskBoardCommand).Execute(_options.TaskboardPath);
			}
		}
	}
}
