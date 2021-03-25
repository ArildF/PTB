using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.ViewModels;

namespace Rogue.Ptb.UI.Adorners
{
	public abstract class TaskboardAdornerBase : Adorner
	{
		private VisualChildPath _path;
	    private static bool _resizing;

// ReSharper disable InconsistentNaming
        private const Int32 WM_SIZING = 0x0214;
	    private const Int32 WM_EXITSIZEMOVE = 0x0232; 
// ReSharper restore InconsistentNaming

	    protected TaskboardAdornerBase(UIElement adornedElement) : base(adornedElement)
		{
		    HookWndProc(adornedElement);
		}

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (!_resizing)
            {
                DoRender(drawingContext);

            }
        }

	    protected abstract void DoRender(DrawingContext dc);

	    private void HookWndProc(UIElement adornedElement)
	    {
	        var window = adornedElement.FindParentOfType<Window>();
	        if (window == null)
	        {
		        return;
	        }

	        var src = (HwndSource)PresentationSource.FromVisual(window);

            if (src != null)
            {
                src.AddHook(WndProc);
            }
	    }

	    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
	    {
	        if (msg == WM_SIZING)
	        {
	            _resizing = true;
	        }
	        else if(msg == WM_EXITSIZEMOVE)
	        {
	            _resizing = false;
                this.InvalidateVisual();
	        }

	        return IntPtr.Zero;
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