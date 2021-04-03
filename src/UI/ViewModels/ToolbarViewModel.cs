using System;
using System.Collections.Generic;
using System.Windows.Input;
using Rogue.Ptb.UI.Commands;
using System.Linq;

namespace Rogue.Ptb.UI.ViewModels
{
	public class ToolbarViewModel : IToolbarViewModel, ICommandResolver
	{
		private readonly ICommandResolver _resolver;
		private readonly ISettings _settings;

		public ToolbarViewModel(ICommandResolver resolver, ISettings settings)
		{
			_resolver = resolver;
			_settings = settings;
		}

		public IEnumerable<MenuItem> LastRecentlyUsedTaskboards
		{
			get { return _settings.LastRecentlyUsedTaskBoards.Select(tb => new MenuItem(_resolver, tb)); }
		}

		public class MenuItem : ICommandResolver
		{
			private readonly ICommandResolver _resolver;
			private readonly string _text;

			public MenuItem(ICommandResolver resolver, string text)
			{
				_resolver = resolver;
				_text = text;
			}

			public string Text
			{
				get { return _text; }
			}

			public ICommand Resolve(CommandName commandName)
			{
				return _resolver.Resolve(commandName);
			}
		}

		public ICommand Resolve(CommandName commandName)
		{
			return _resolver.Resolve(commandName);
		}
	}

}
