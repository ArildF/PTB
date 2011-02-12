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


	public class CreateTaskBoardDialogResult : DialogReturnValueBase<Views.CreateTaskBoardDialog, DialogArgsBase>
	{
		public string Path { get; private set; }

		public CreateTaskBoardDialogResult(string path)
		{
			Path = path;
		}
	}
}
