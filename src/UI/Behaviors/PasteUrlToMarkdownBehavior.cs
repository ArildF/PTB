using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Xaml.Behaviors;
using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.Behaviors
{
	public class PasteUrlToMarkdownBehavior : Behavior<TextBox>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			var pasteBinding = new CommandBinding(
				ApplicationCommands.Paste,
				PasteExecuted,
				PasteCanExecute);
			AssociatedObject.CommandBindings.Add(pasteBinding);
		}

		private void PasteCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if (Clipboard.ContainsText() && Uri.TryCreate(Clipboard.GetText(), UriKind.Absolute, out _))
			{
				e.CanExecute = true;
				e.Handled = true;
			}
		}

		private void PasteExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			var url = Clipboard.GetText();
			AssociatedObject.SelectedText = $"[{url}]({url})";

			AssociatedObject.SelectionStart = AssociatedObject.SelectionStart + 1;
			AssociatedObject.SelectionLength = url.Length;
		}
	}
}