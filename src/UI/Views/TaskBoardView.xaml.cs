using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Expression.Interactivity.Layout;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Views
{
	/// <summary>
	/// Interaction logic for TaskBoardView.xaml
	/// </summary>
	public partial class TaskBoardView : ITaskBoardView
	{
		public TaskBoardView()
		{
			InitializeComponent();
		}

		public TaskBoardView(ITaskBoardViewModel vm) : this()
		{
			DataContext = vm;
		}

		public UIElement Element
		{
			get { return this; }
		}

		private void MouseDragElementBehaviorOnDragFinished(object sender, MouseEventArgs args)
		{
			var draggedElement = (UIElement)((IAttachedObject) sender).AssociatedObject;
			var position = args.GetPosition(_itemsControl);

			IInputElement elt= _itemsControl.InputHitTest(position);
			var elt2 = elt as UIElement;

			if (elt2 == null)
			{
				draggedElement.RenderTransform = Transform.Identity;
				return;
			}

			var grid = elt2.TraverseBy(e => (UIElement) VisualTreeHelper.GetParent(e)).OfType<Grid>().FirstOrDefault();

			if (grid == null)
			{
				return;
			}


			double total = 0;
			position = args.GetPosition(grid);

			for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
			{
				var coldef = grid.ColumnDefinitions[i];
				total += coldef.ActualWidth;

				if (i.In(1, 3))
				{
					continue;
				}
				if (position.X < total)
				{
					Grid.SetColumn(draggedElement, i);
					draggedElement.RenderTransform = Transform.Identity;
					break;
				}
			}
		}
	}

}
