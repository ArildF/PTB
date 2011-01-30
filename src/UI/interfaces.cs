

using System.Windows;
using System.Windows.Controls;

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
}
