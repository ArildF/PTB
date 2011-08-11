using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Rogue.Ptb.UI.ViewModels;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.UI.Adorners
{
	public class SubtasksAdorner : TaskboardAdornerBase
	{
		private readonly Pen _pen = new Pen(Brushes.Gray, 0.75);

		public SubtasksAdorner(UIElement adornedElement) : base(adornedElement)
		{
		}

		protected override void DoRender(DrawingContext dc)
		{
			foreach (var tuple in FindSuperTasks())
			{
				FrameworkElement parentTaskElement = tuple.Item1;
				var startPoint = FindPoint(parentTaskElement, new Point(0, parentTaskElement.ActualHeight/2));

				startPoint.Offset(0, tuple.Item2.IndentLevel * 3);

				var endpoints = (from vm in tuple.Item2.ChildVMs()
				                 let element = FindElement(vm)
				                 let border = FindBorder(element)
				                 select FindPoint(border, new Point(0, border.ActualHeight/2))).OrderBy(pt => pt.Y).ToArray();

				var leftMost = endpoints.Concat(new[]{startPoint}).Min(point => point.X);
				var bottomMost = endpoints.Last();

				var builder = BuildPath.From(startPoint)
					.LineTo(leftMost - 3, startPoint.Y)
					.CurveTo(leftMost - 8, startPoint.Y + 5, SweepDirection.Counterclockwise)
					.LineTo(leftMost - 8, bottomMost.Y - 5)
					.CurveTo(leftMost - 3, bottomMost.Y, SweepDirection.Counterclockwise)
					.LineTo(bottomMost.X, bottomMost.Y);

				foreach (var endpoint in endpoints.Take(endpoints.Count() - 1))
				{
					builder.NewFigureFrom(leftMost - 8, endpoint.Y).LineTo(endpoint.X, endpoint.Y);
				}

				var path = builder.Build();

				dc.DrawGeometry(null, _pen, path);
			}

		}

		private IEnumerable<Tuple<FrameworkElement, TaskViewModel>> FindSuperTasks()
		{
			var result = from viewModel in GetViewModels()
			             let presenter =
			             	FrameworkElementFromItem(viewModel)
			             where viewModel.IsVisible && viewModel.CanCollapse
						 let uiElement = FindBorder(presenter)
			             select Tuple.Create(uiElement, viewModel);

			return result;

		}
	}
}
