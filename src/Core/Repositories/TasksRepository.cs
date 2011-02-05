using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Rogue.Ptb.Core.Repositories
{
	public class TasksRepository : RepositoryBase<Task>, ITasksRepository
	{
		public TasksRepository(ISession session) : base(session)
		{
		}
	}
}
