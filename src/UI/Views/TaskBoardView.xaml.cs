using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Documents;
using NHibernate.Util;
using Rogue.Ptb.UI.Adorners;

namespace Rogue.Ptb.UI.Views
{
	/// <summary>
	/// Interaction logic for TaskBoardView.xaml
	/// </summary>
	public partial class TaskBoardView : ITaskBoardView
	{
		private SubtasksAdorner _subtasksAdorner;
		private  TaskPriorityAdorner _taskPriorityAdorner;

		public TaskBoardView()
		{
			InitializeComponent();

			Loaded += OnLoaded;

			this.Events().LayoutUpdated
				.Buffer(2) // skip layout updates caused by InvalidateAdorners
				.ObserveOnDispatcher()
				.Subscribe(_ => InvalidateAdorners());
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			var layer = AdornerLayer.GetAdornerLayer(_itemsControl);
			_subtasksAdorner = new SubtasksAdorner(_itemsControl);
			_taskPriorityAdorner = new TaskPriorityAdorner(_itemsControl);
			
			layer.Add(_subtasksAdorner);
			layer.Add(_taskPriorityAdorner);
		}

		public TaskBoardView(ITaskBoardViewModel vm) : this()
		{
			DataContext = vm;
		}

		public UIElement Element => this;

		private void InvalidateAdorners()
		{
			GetAdorners().Where(ad => ad != null).ForEach(ad => ad.InvalidateVisual());
		}


		private IEnumerable<Adorner> GetAdorners()
		{
			yield return _subtasksAdorner;
			yield return _taskPriorityAdorner;
		}
	}

}
