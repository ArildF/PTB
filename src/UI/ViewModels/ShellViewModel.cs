using System.IO;
using System.Windows.Input;
using ReactiveUI;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Commands;
using System;

namespace Rogue.Ptb.UI.ViewModels
{
	public class ShellViewModel : ViewModelBase, IShellViewModel, ICommandResolver
	{
		private readonly ICommandResolver _resolver;
		private readonly IEventAggregator _eventAggregator;
		private string _title;

		public ShellViewModel(ICommandResolver resolver, IEventAggregator eventAggregator)
		{
			_resolver = resolver;
			_eventAggregator = eventAggregator;

			_eventAggregator.ListenOnScheduler<DatabaseChanged>(OnDatabaseChanged);

			Title = "Personal Task Board - (no task board loaded)";
		}

		private void OnDatabaseChanged(DatabaseChanged evt)
		{
			string name = Path.GetFileNameWithoutExtension(evt.Path);
			Title = String.Format("{0} - Personal Task Board", name);
		}

		public string Title
		{
			get { return _title; }
			set { this.RaiseAndSetIfChanged(ref _title, value); }
		}

		public ICommand Resolve(CommandName commandName)
		{
			return _resolver.Resolve(commandName);
		}
	}

}
