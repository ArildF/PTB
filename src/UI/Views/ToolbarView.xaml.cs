using System;
using System.Windows;
using System.Windows.Controls;
using Rogue.Ptb.UI.ViewModels;

namespace Rogue.Ptb.UI.Views
{
	/// <summary>
	/// Interaction logic for ToolbarView.xaml
	/// </summary>
	public partial class ToolbarView : IToolbarView
	{
		public ToolbarView()
		{
			InitializeComponent();
		}

		public ToolbarView(IToolbarViewModel vm) : this()
		{
			DataContext = vm;
		}

		public UIElement Element
		{
			get { return this; }
		}
	}

}
