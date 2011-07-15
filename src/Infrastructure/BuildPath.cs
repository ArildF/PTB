using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Rogue.Ptb.Infrastructure
{
	public class BuildPath
	{
		public interface IPathBuilder
		{
			IPathBuilder LineTo(double x, double y);
			IPathBuilder CurveTo(double x, double y, SweepDirection direction);
			PathGeometry Build();
			IPathBuilder NewFigureFrom(double x, double y);
		}

		public static IPathBuilder From(double x, double y)
		{
			return new PathBuilder(x, y);
		}

		private class PathBuilder : IPathBuilder
		{
			private Point _startPoint;
			private readonly List<PathFigure> _figures = new List<PathFigure>(); 
			private List<PathSegment> _segments = new List<PathSegment>();
			private bool _currentIsStroked;

			private Point _currentPoint;

			public PathBuilder(double x, double y)
			{
				_currentPoint = _startPoint = new Point(x, y);
				_currentIsStroked = true;
			}

			public IPathBuilder LineTo(double x, double y)
			{
				_currentPoint = new Point(x, y);
				_segments.Add(new LineSegment(_currentPoint, _currentIsStroked));
				return this;
			}

			public IPathBuilder CurveTo(double x, double y, SweepDirection direction)
			{
				var newPoint = new Point(x, y);
				var diff = Point.Subtract(_currentPoint, newPoint);
				var radius = new Size(Math.Abs(diff.X), Math.Abs(diff.Y));

				_segments.Add(new ArcSegment(new Point(x, y), radius, 45, false, direction, _currentIsStroked));
				return this;
			}

			public PathGeometry Build()
			{
				FinalizeFigure();
				return new PathGeometry(_figures);
			}

			public IPathBuilder NewFigureFrom(double x, double y)
			{
				FinalizeFigure();
				_startPoint = new Point(x, y);
				return this;
			}

			private void FinalizeFigure()
			{
				_figures.Add(new PathFigure(_startPoint, _segments, false));

				_segments = new List<PathSegment>();
			}
		}

		public static IPathBuilder From(Point startPoint)
		{
			return new PathBuilder(startPoint.X, startPoint.Y);
		}
	}
}
