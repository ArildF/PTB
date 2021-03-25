using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using NHibernate.Util;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Adorners;

namespace Rogue.Ptb.UI.Views
{
	/// <summary>
	/// Interaction logic for TaskBoardView.xaml
	/// </summary>
	public partial class TaskBoardView : ITaskBoardView
	{
		private readonly IEventAggregator _aggregator;
		private SubtasksAdorner _subtasksAdorner;
		private  TaskPriorityAdorner _taskPriorityAdorner;

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
			_taskPriorityAdorner = new TaskPriorityAdorner(_itemsControl);
			
			layer.Add(_subtasksAdorner);
			layer.Add(_taskPriorityAdorner);
		}

		public TaskBoardView(ITaskBoardViewModel vm, IEventAggregator aggregator) : this()
		{
			_aggregator = aggregator;
			DataContext = vm;

			_aggregator.Listen<TaskStateChanged>().Delay(TimeSpan.FromMilliseconds(50))
				.ObserveOnDispatcher()
				.ObserveOnIdle()
				.Subscribe(_ => InvalidateAdorners());
		}

		public UIElement Element
		{
			get { return this; }
		}

		private void OnTaskSelectionAnimationCompleted(object sender, EventArgs e)
		{
			InvalidateAdorners();
		}

		private void InvalidateAdorners()
		{
			GetAdorners().ForEach(ad => ad.InvalidateVisual());
		}


		private IEnumerable<Adorner> GetAdorners()
		{
			yield return _subtasksAdorner;
			yield return _taskPriorityAdorner;
		}
	}

}
