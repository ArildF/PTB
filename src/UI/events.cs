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

	public class CollapseAll : ICommandEvent{}
}
