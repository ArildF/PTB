namespace Rogue.Ptb.UI
{
	public class CreateNewTask : ICommandEvent
	{
		
	}

	public class CreateNewSubTask : ICommandEvent{}

	public class SaveAllTasks : ICommandEvent{}

	public class ReloadAllTasks : ICommandEvent{}

	public class ReSort : ICommandEvent {}

	public class ApplicationIdle {}

	public class TaskStateChanged{}
	
	public class TaskModified{}

	public class CollapseAll : ICommandEvent{}

	public class CommandCanExecute<T>
	{
		public CommandCanExecute(bool canExecute)
		{
			CanExecute = canExecute;
		}

		public bool CanExecute { get; }
	}
}
