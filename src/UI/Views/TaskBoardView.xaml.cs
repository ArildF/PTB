using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Adorners;

namespace Rogue.Ptb.UI.Views
{
	/// <summary>
	/// Interaction logic for TaskBoardView.xaml
	/// </summary>
	public partial class TaskBoardView : ITaskBoardView
	{
		private SubtasksAdorner _subtasksAdorner;

		public TaskBoardView()
		{
			InitializeComponent();

			Loaded += OnLoaded;

			var board = (Storyboard)_itemsControl.ItemTemplate.Resources["OnSelected"];
			board.Completed += OnTaskSelectionAnimationCompleted;

			board = (Storyboard) _itemsControl.ItemTemplate.Resources["OnDeselected"];
			board.Completed += OnTaskSelectionAnimationCompleted;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			var layer = AdornerLayer.GetAdornerLayer(_itemsControl);
			_subtasksAdorner = new SubtasksAdorner(_itemsControl);
			layer.Add(_subtasksAdorner);
		}

		public TaskBoardView(ITaskBoardViewModel vm) : this()
		{
			DataContext = vm;
		}

		public UIElement Element
		{
			get { return this; }
		}

		private void OnTaskSelectionAnimationCompleted(object sender, EventArgs e)
		{
			_subtasksAdorner.InvalidateVisual();
		}
	}

}
