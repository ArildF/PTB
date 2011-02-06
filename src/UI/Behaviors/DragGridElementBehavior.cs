using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Behaviors
{
	public class DragGridElementBehavior : Behavior<Grid>
	{
		private Point _startPoint;
		private UIElement _movedObject;
		private Transform _originalTransform;

		public static readonly DependencyProperty Draggable = DependencyProperty.RegisterAttached(
			"DraggableProperty", typeof (bool), typeof (DragGridElementBehavior));

		public static bool GetDraggable(DependencyObject d)
		{
			return (bool)d.GetValue(Draggable);
		}

		public static void SetDraggable(DependencyObject d, bool value)
		{
			d.SetValue(Draggable, value);
		}

		public DragGridElementBehavior()
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

			_startPoint = e.GetPosition(RootElement);
			_originalTransform = _movedObject.RenderTransform;
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
			GridLocation location = FindGridLocation(position);
			if (!location.IsEmpty)
			{
				Grid.SetColumn(_movedObject, location.X);
				Grid.SetRow(_movedObject, location.Y);
			}

			_movedObject.RenderTransform = _originalTransform;
		}

		private GridLocation FindGridLocation(Point position)
		{
			if (position.X < 0 || position.X > AssociatedObject.ActualWidth ||
				position.Y < 0 || position.Y > AssociatedObject.ActualHeight)

			{
				return GridLocation.MakeEmpty();
			}

			int x = 0, y = 0;
			double columnRight = 0, rowTop = 0;
			for (int i = 0; i < AssociatedObject.ColumnDefinitions.Count; i++)
			{
				var cd = AssociatedObject.ColumnDefinitions[i];

				columnRight += cd.ActualWidth;
				if (position.X <= columnRight)
				{
					x = i;
					break;
				}
			}
			for (int i = 0; i < AssociatedObject.RowDefinitions.Count; i++)
			{
				var cd = AssociatedObject.RowDefinitions[i];

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
