using System.Windows;
using Rogue.Ptb.UI.ViewModels;
using SourceChord.FluentWPF;

namespace Rogue.Ptb.UI.Views
{
	public partial class NotesWindow 
	{
		public NotesWindow()
		{
			InitializeComponent();
		}

		public NotesWindow(NotesDisplayViewModel vm) : this()
		{
			DataContext = vm;
		}
	}
}