using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using Rogue.Ptb.Infrastructure;

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
