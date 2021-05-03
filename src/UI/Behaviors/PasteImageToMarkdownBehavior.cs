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
	public class PasteImageToMarkdownBehavior : Behavior<TextBox>
	{
		public static readonly DependencyProperty NoteProperty = DependencyProperty.Register(
			"Note", typeof(IAmMarkdownNote), typeof(PasteImageToMarkdownBehavior), new PropertyMetadata(default(IAmMarkdownNote)));

		public IAmMarkdownNote Note
		{
			get { return (IAmMarkdownNote) GetValue(NoteProperty); }
			set { SetValue(NoteProperty, value); }
		}
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
			if (Clipboard.ContainsImage())
			{
				e.CanExecute = true;
				e.Handled = true;
			}
		}

		private async void PasteExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			if (Note == null)
			{
				return;
			}
			var image = Clipboard.GetImage();
			if (image == null)
			{
				return;
			}
			
			PngBitmapEncoder enc = new PngBitmapEncoder();
			using (var stream = new MemoryStream())
			{
				enc.Frames.Add(BitmapFrame.Create(image));
				enc.Save(stream);
				var attachment = new Attachment
				{
					Id = Guid.NewGuid(),
					ContentType = "image/png",
					Content = stream.GetBuffer(),
					Name = "Pasted image",
				};
				await Note.AddAttachment(attachment);
				AssociatedObject.SelectedText = $"![ResourceImage](ptbnoteattachment://{attachment.Id})";
			}

		}
	}
}