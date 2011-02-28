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

			_eventAggregator.Listen<DatabaseChanged>().Subscribe(OnDatabaseChanged);

			Title = "Personal Task Board - (no task board loaded)";
		}

		private void OnDatabaseChanged(DatabaseChanged evt)
		{
			Title = String.Format("Personal Task Board - {0}", evt.Path);
		}

		public string Title
		{
			get { return _title; }
			set { this.RaiseAndSetIfChanged(vm => vm.Title, value); }
		}

		public ICommand Resolve(CommandName commandName)
		{
			return _resolver.Resolve(commandName);
		}
	}

}
