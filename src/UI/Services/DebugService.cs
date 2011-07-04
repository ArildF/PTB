using System.Diagnostics;
using Rogue.Ptb.UI.Views;

namespace Rogue.Ptb.UI.Services
{
	public class DebugService : IDebugService
	{
		public void Dump(string str)
		{
			if (Debugger.IsAttached)
			{
				Trace.WriteLine(str);
			}
			else
			{
				var dialog = new DebugDumpDialog(str);
				dialog.ShowDialog();
			}
		}
	}
}
