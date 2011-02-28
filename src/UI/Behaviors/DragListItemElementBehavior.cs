using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Behaviors
{
	public class DragListItemElementBehavior : Behavior<ItemsControl>
	{
		private Point _startPoint;
		private UIElement _movedObject;
		private Transform _originalTransform;
		private int _originalZIndex;

		public static readonly DependencyProperty Draggable = DependencyProperty.RegisterAttached(
			"DraggableProperty", typeof (bool), typeof (DragListItemElementBehavior));

		public static bool GetDraggable(DependencyObject d)
		{
			return (bool)d.GetValue(Draggable);
		}

		public static void SetDraggable(DependencyObject d, bool value)
		{
			d.SetValue(Draggable, value);
		}

		public static readonly DependencyProperty DragTargetCommand = DependencyProperty.RegisterAttached(
			"DragTargetCommand", typeof (ICommand), typeof (DragListItemElementBehavior));

		private FrameworkElement _movedSource;

		public static ICommand GetDragTargetCommand(DependencyObject d)
		{
			return (ICommand) d.GetValue(DragTargetCommand);
		}

		public static void SetDragTargetCommand(DependencyObject d, ICommand value)
		{
			d.SetValue(DragTargetCommand, value);
		}

		public static readonly DependencyProperty IsDragTargetGrid = DependencyProperty.RegisterAttached(
			"IsDragTargetGrid", typeof (bool), typeof (DragListItemElementBehavior));

		public static bool GetIsDragTargetGrid(Grid d)
		{
			return (bool) d.GetValue(IsDragTargetGrid);
		}

		public static void SetIsDragTargetGrid(Grid d, bool value)
		{
			d.SetValue(IsDragTargetGrid, value);
		}

		public DragListItemElementBehavior()
		{
			AllElementsDraggable = true;
		}

		public bool AllElementsDraggable { get; set; }

		protected override void OnAttached()
		{
			AssociatedObject.MouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.MouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
		}

		private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			var gridRelativeStartPoint = e.GetPosition(AssociatedObject);

			DependencyObject foundObject = null;
			VisualTreeHelper.HitTest(AssociatedObject,
				d =>
					{
						if (AllElementsDraggable || GetDraggable(d))
						{
							foundObject = d;
							return HitTestFilterBehavior.Stop;
						}
						return HitTestFilterBehavior.Continue;
					},
				htr => HitTestResultBehavior.Continue,
					new PointHitTestParameters(gridRelativeStartPoint)
				);

			if (foundObject == null)
			{
				return;
			}

			_movedObject = foundObject as UIElement;

			if (_movedObject == null || !(AllElementsDraggable || GetDraggable(_movedObject)))
			{
				return;
			}

			_movedSource = GetDragCommandAndTarget(gridRelativeStartPoint, true).Item2;

			_startPoint = e.GetPosition(RootElement);
			_originalTransform = _movedObject.RenderTransform;

			_originalZIndex = Panel.GetZIndex(_movedObject);
			Panel.SetZIndex(_movedObject, 1000);

			AssociatedObject.CaptureMouse();
			AssociatedObject.MouseMove += OnMouseMove;
			AssociatedObject.MouseLeftButtonUp += OnMouseUp;

			e.Handled = true;
		}

		protected UIElement RootElement
		{
			get
			{
				return AssociatedObject.TraverseBy<UIElement>(o => (UIElement) VisualTreeHelper.GetParent(o))
					.Where(elt => elt != null).Last();
			}
		}

		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			var currentPoint = e.GetPosition(RootElement);

			var translation = _originalTransform.Value;
			translation.Translate(currentPoint.X - _startPoint.X, currentPoint.Y - _startPoint.Y);
			
			_movedObject.RenderTransform = new MatrixTransform(
				translation);
		}


		private void OnMouseUp(object sender, MouseButtonEventArgs e)
		{
			AssociatedObject.MouseMove -= OnMouseMove;
			AssociatedObject.MouseLeftButtonUp -= OnMouseUp;
			AssociatedObject.ReleaseMouseCapture();

			var position = e.GetPosition(AssociatedObject);

			// first, check for up and down drag
			var dragCommandAndTarget = GetDragCommandAndTarget(position, false);

			if (dragCommandAndTarget.Item1 != null)
			{
				var contextMovedObject = _movedObject.GetValue(FrameworkElement.DataContextProperty);
				var contextTargetObject = dragCommandAndTarget.Item2.GetValue(FrameworkElement.DataContextProperty);

				var args = new DragCommandArgs(contextMovedObject, contextTargetObject);

				if (dragCommandAndTarget.Item1.CanExecute(args))
				{
					dragCommandAndTarget.Item1.Execute(args);
				}
			}
			else
			{
				// now check for sideways drag
				var grid = FindDragTargetGrid(position);

				if (grid != null)
				{
					MoveSideways(grid, e.GetPosition(grid));
				}
			}

			_movedObject.RenderTransform = _originalTransform;
			Panel.SetZIndex(_movedObject, _originalZIndex);
			_movedSource = null;
		}

		private Grid FindDragTargetGrid(Point position)
		{
			Grid foundGrid = null;

			VisualTreeHelper.HitTest(AssociatedObject, v =>
				{
					var grid = v as Grid;

					if (grid == null || !GetIsDragTargetGrid(grid))
					{
						return HitTestFilterBehavior.Continue;
					}
					foundGrid = grid;
					return HitTestFilterBehavior.Stop;
				}, 
				htr => HitTestResultBehavior.Continue,
				new PointHitTestParameters(position));
			return foundGrid;
		}

		private void MoveSideways(Grid grid, Point position)
		{
			GridLocation location = FindGridLocation(grid, position);
			if (!location.IsEmpty)
			{
				Grid.SetColumn(_movedObject, location.X);
				Grid.SetRow(_movedObject, location.Y);
			}
		}

		private Tuple<ICommand, FrameworkElement> GetDragCommandAndTarget(Point position, bool acceptSameAsSource)
		{
			var retVal = Tuple.Create<ICommand, FrameworkElement>(null, null);


			VisualTreeHelper.HitTest(AssociatedObject, d =>
				{
					var cmd = GetDragTargetCommand(d);
					if (cmd != null && d is FrameworkElement && (acceptSameAsSource || d != _movedSource))
					{
						retVal = Tuple.Create(cmd, d as FrameworkElement);

					}

					return HitTestFilterBehavior.Continue;
				}, htr => HitTestResultBehavior.Continue,
			                         new PointHitTestParameters(position));

			return retVal;
		}

		private GridLocation FindGridLocation(Grid grid, Point position)
		{
			if (position.X < 0 || position.X > grid.ActualWidth ||
				position.Y < 0 || position.Y > grid.ActualHeight)
			{
				return GridLocation.MakeEmpty();
			}

			int x = 0, y = 0;
			double columnRight = 0, rowTop = 0;
			for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
			{
				var cd = grid.ColumnDefinitions[i];

				columnRight += cd.ActualWidth;
				if (position.X <= columnRight)
				{
					x = i;
					break;
				}
			}
			for (int i = 0; i < grid.RowDefinitions.Count; i++)
			{
				var cd = grid.RowDefinitions[i];

				rowTop += cd.ActualHeight;
				if (position.Y <= rowTop)
				{
					y = i;
					break;
				}
			}
			return new GridLocation(x, y);
		}

		private struct GridLocation
		{
			public readonly int X;
			public readonly int Y;

			public GridLocation(int x, int y)
			{
				X = x;
				Y = y;
			}

			public bool IsEmpty
			{
				get { return X < 0 || Y < 0; }
			}

			public static GridLocation MakeEmpty()
			{
				return new GridLocation(-1, -1);
			}
		}
	}
}
