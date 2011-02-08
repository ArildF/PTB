using NHibernate;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.Core.Repositories
{
	public class TasksRepository : RepositoryBase<Task>, ITasksRepository
	{
		public TasksRepository(ISession session) : base(session)
		{
		}
	}
}
