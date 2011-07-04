namespace Rogue.Ptb.UI.Commands
{
	public static class GlobalCommandNames
	{
		public static CommandName CreateTaskBoardCommand = CommandName.Create<CreateTaskBoard>();
		public static CommandName ExportTaskBoardCommand = CommandName.Create<ExportTaskBoard>();
		public static CommandName ImportTaskBoardCommand = CommandName.Create<ImportTaskBoard>();
		public static CommandName CreateNewTaskCommand = CommandName.Create<CreateNewTask>();
		public static CommandName DebugDumpImportantLinks = CommandName.Create<DebugDumpImportantLinks>();
	}
}
