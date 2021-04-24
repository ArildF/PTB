using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Windows.Input;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Commands
{
	public abstract class CommandBase : ICommand
	{
		public virtual void Execute(object parameter)
		{
			
		}

		public virtual bool CanExecute(object parameter)
		{
			return true;
		}

		protected virtual void OnCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}

		public event EventHandler CanExecuteChanged;
	}

	[DebuggerDisplay("{GetType()}")]
	public abstract class NoParameterCommandBase<T> : ICommand
	{
		private bool _canExecute = true;

		protected NoParameterCommandBase(IEventAggregator bus)
		{
			bus.Listen<CommandCanExecute<T>>()
				.ObserveOnDispatcher()
				.Do(cce => _canExecute = cce.CanExecute)
				.Subscribe(_ => CanExecuteChanged?.Invoke(this, EventArgs.Empty));

		}

		void ICommand.Execute(object parameter)
		{
			Execute();
		}

		public event EventHandler? CanExecuteChanged;

		protected virtual void Execute()
		{}

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute();
		}

		protected virtual bool CanExecute()
		{
			return _canExecute;
		}

	}

	public abstract class CommandBase<T> : ICommand
	{
		void ICommand.Execute(object parameter)
		{
			Execute((T) parameter);

		}

		public event EventHandler? CanExecuteChanged;

		protected virtual void Execute(T parameter)
		{
		}

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute((T) parameter);
		}

		private bool CanExecute(T parameter)
		{
			return true;
		}

	}
}
