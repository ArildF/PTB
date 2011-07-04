

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Rogue.Ptb.UI.Commands;

namespace Rogue.Ptb.UI
{
	public interface IComponentView
	{
		UIElement Element { get; }
	}

	public interface IToolbarView : IComponentView
	{
	}

	public interface IToolbarViewModel
	{
	}

	public interface IShellViewModel
	{
	}

	public interface ITaskBoardViewModel
	{
	}

	public interface ITaskBoardView : IComponentView
	{
	}

	public interface IShellView
	{
		Window Window { get; }
	}

	public interface ICommandResolver
	{
		ICommand Resolve(CommandName commandName);
	}

	public interface ICommandEvent {}

	public interface IReactivePropertyChangingObject
	{
		void OnPropertyChanging(string propertyName);
		void OnPropertyChanged(string propertyName);

	}

	public interface IDialogDisplayer
	{
		TDialogReturnValue ShowDialogFor<TDialogReturnValue>(DialogArgsBase args = null) 
			where TDialogReturnValue : DialogReturnValueBase;
	}

	public interface ISettings : IStartable
	{
		IEnumerable<string> LastRecentlyUsedTaskBoards { get; }
	}

	public interface IStartable
	{
		void Start();
	}

	public interface IDebugService
	{
		void Dump(string str);

	}
}
