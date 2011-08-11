using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Linq;

namespace Rogue.Ptb.Infrastructure
{
	public static class WpfExtensions
	{
		public static FrameworkElement FindVisualChild<T>(this UIElement element, string name) where T:FrameworkElement
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(element, i);
				FrameworkElement elt = child as T;
				if (elt != null && elt.Name == name)
				{
					return elt;
				}
				elt = FindVisualChild<T>(elt, name);
				if (elt != null)
				{
					return elt;
				}
			}
			return null;
		}

		public static VisualChildPath FindVisualChildPath<T>(this UIElement element, string name)  where T:FrameworkElement
		{
			var stack = new Stack<int>();

			if (DoFindVisualChildPath<T>(element, name, stack))
			{
				return new VisualChildPath(stack.Reverse());
			}

			return null;

		}

        public static T FindParentOfType<T>(this DependencyObject obj) where T: DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(obj);
            if (parent == null)
            {
                return null;
            }

            if (parent is T)
            {
                return (T) parent;
            }

            return FindParentOfType<T>(parent);
        }

		private static bool DoFindVisualChildPath<T>(DependencyObject element, string name, Stack<int> stack) where T:FrameworkElement
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
			{
				stack.Push(i);

				DependencyObject child = VisualTreeHelper.GetChild(element, i);
				FrameworkElement elt = child as T;
				if (elt != null && elt.Name == name)
				{
					return true;
				}
				if(DoFindVisualChildPath<T>(child, name, stack))
				{
					return true;
				}

				stack.Pop();
			}
			return false;
		}
	}

	public class VisualChildPath
	{
		private readonly IEnumerable<int> _path;

		public VisualChildPath(IEnumerable<int> path)
		{
			_path = path;
		}

		public FrameworkElement FindElement(DependencyObject inReferenceTo)
		{
			DependencyObject current = inReferenceTo;
			foreach (var i in _path)
			{
				current = VisualTreeHelper.GetChild(current, i);
			}
			return current as FrameworkElement;
		}
	}
}
