using System;
using System.Collections.Generic;
using System.Windows.Input;
using Rogue.Ptb.UI.Commands;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.ViewModels
{
	public class ToolbarViewModel : ViewModelBase, IToolbarViewModel, ICommandResolver
	{
		private readonly ICommandResolver _resolver;
		private readonly ISettings _settings;

		public ToolbarViewModel(ICommandResolver resolver, ISettings settings, IEventAggregator aggregator)
		{
			_resolver = resolver;
			_settings = settings;

			aggregator.Listen<DatabaseChanged>()
				.Throttle(TimeSpan.FromSeconds(0.5))
				.ObserveOnDispatcher()
				.Subscribe(_ => this.RaisePropertyChanged(v => v.LastRecentlyUsedTaskboards));
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
