using System.Windows;
using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.ViewModels
{
	public class NotesDisplayViewModel : ViewModelBase
	{
		private readonly Task _task;

		public NotesDisplayViewModel(Task task)
		{
			_task = task;
		}

		public string Title => _task.Title;

		public string Markdown => @"# This is a header #
## This is another header ##
This is not a header.

This is __underlined__.

This is **bold**

```cs
	var canExecute = from ea in Observable.FromEventPattern<TextChangedEventArgs>(_pathTextBox, ""TextChanged"")
		let path = _pathTextBox.Text
		select File.Exists(path);
```
That was some code :point_up:

> this is a quote";
		
	}
}