using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.ViewModels;

namespace Rogue.Ptb.UI.Adorners
{
	public class TaskboardAdornerBase : Adorner
	{
		private VisualChildPath _path;

		public TaskboardAdornerBase(UIElement adornedElement) : base(adornedElement)
		{
		}

		protected ItemsControl AdornedItemsControl
		{
			get { return (ItemsControl) AdornedElement; }
		}

		protected Point FindPoint(FrameworkElement border, Point point)
		{
			var transform = border.TransformToAncestor((Visual) AdornedItemsControl);
			return transform.Transform(point);
		}

		protected FrameworkElement FindElement(TaskViewModel vm)
		{
			return (FrameworkElement) AdornedItemsControl.ItemContainerGenerator.ContainerFromItem(vm);
		}

		protected FrameworkElement FindBorder(FrameworkElement presenter)
		{
			if (_path == null)
			{
				_path = presenter.FindVisualChildPath<Border>("_border");
			}
			return _path.FindElement(presenter);
		}

		protected IEnumerable<TaskViewModel> GetViewModels()
		{
			return AdornedItemsControl.Items.Cast<TaskViewModel>();
		}

		protected FrameworkElement FrameworkElementFromItem(TaskViewModel viewModel)
		{
			return (FrameworkElement) AdornedItemsControl.ItemContainerGenerator.ContainerFromItem(viewModel);
		}
	}
}