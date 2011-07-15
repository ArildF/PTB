using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Adorners;

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

			Loaded += OnLoaded;
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			var layer = AdornerLayer.GetAdornerLayer(_itemsControl);
			layer.Add(new SubtasksAdorner(_itemsControl));
		}

		public TaskBoardView(ITaskBoardViewModel vm) : this()
		{
			DataContext = vm;
		}

		public UIElement Element
		{
			get { return this; }
		}
	}

}
