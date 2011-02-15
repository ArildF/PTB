using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ReactiveUI;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI
{
	public class Settings : ISettings
	{
		private readonly Properties.Settings _settings;
		private readonly IEventAggregator _eventAggregator;

		public Settings(Properties.Settings settings, IEventAggregator eventAggregator)
		{
			_settings = settings;
			_eventAggregator = eventAggregator;
		}

		public void Start()
		{
			_eventAggregator.ListenOnScheduler<DatabaseChanged>(OnDatabaseChanged);
		}

		private void OnDatabaseChanged(DatabaseChanged databaseChanged)
		{
			if (_settings.LastRecentlyUsedTaskboards == null)
			{
				_settings.LastRecentlyUsedTaskboards = new StringCollection();
			}
			if (!LastRecentlyUsedTaskBoards.Any(tb => tb.Equals(databaseChanged.Path, StringComparison.CurrentCultureIgnoreCase)))
			{
				_settings.LastRecentlyUsedTaskboards.Insert(0, databaseChanged.Path);
				_settings.Save();
			}
		}

		public IEnumerable<string> LastRecentlyUsedTaskBoards
		{
			get { return (_settings.LastRecentlyUsedTaskboards ?? 
				(_settings.LastRecentlyUsedTaskboards = new StringCollection())).Cast<string>(); }
		}
	}
}
