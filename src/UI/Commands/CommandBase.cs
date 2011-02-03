using System;
using System.Windows.Input;

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

	public abstract class NoParameterCommandBase : ICommand
	{
		void ICommand.Execute(object parameter)
		{
			Execute();
		}

		protected virtual void Execute()
		{}

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute();
		}

		protected virtual bool CanExecute()
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;
	}

	public abstract class CommandBase<T> : ICommand
	{
		void ICommand.Execute(object parameter)
		{
			Execute((T) parameter);

		}

		protected virtual void Execute(T parameter)
		{
		}

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute((T) parameter);
		}

		protected virtual bool CanExecute(T parameter)
		{
			return true;
		}

		public event EventHandler CanExecuteChanged;
	}
}
