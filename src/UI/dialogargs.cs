using System;
using System.Windows.Controls;

namespace Rogue.Ptb.UI
{
	public class DialogReturnValueBase{}

	public class DialogReturnValueBase<TDialog, TArgs> : DialogReturnValueBase
		where TDialog : UserControl
		where TArgs : DialogArgsBase
	{
		
	}

	public class DialogArgsBase
	{}

	public class PathDialogResult<TDialog> : DialogReturnValueBase<TDialog, DialogArgsBase> where TDialog : UserControl
	{
		protected PathDialogResult(string path)
		{
			Path = path;
		}

		public string Path { get; private set; }
	}

	public class CreateTaskBoardDialogResult : PathDialogResult<Views.CreateTaskBoardDialog>
	{
		public CreateTaskBoardDialogResult(string path) : base(path)
		{
		}
	}

	public class OpenTaskBoardDialogResult : PathDialogResult<Views.OpenTaskBoardDialog>
	{
		public OpenTaskBoardDialogResult(string path) : base(path)
		{
		}
	}

	public class ExportTaskBoardDialogResult : PathDialogResult<Views.ExportTaskBoardDialog>
	{
		public ExportTaskBoardDialogResult(string path) : base(path)
		{
			
		}
	}

	public class ImportTaskBoardDialogResult : PathDialogResult<Views.ImportTaskBoardDialog>
	{
		public ImportTaskBoardDialogResult(string path) : base(path)
		{
			
		}
	}
}
