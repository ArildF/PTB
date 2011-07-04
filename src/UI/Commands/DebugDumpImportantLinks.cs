using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Rogue.Ptb.Core;

namespace Rogue.Ptb.UI.Commands
{
	public class DebugDumpImportantLinks : NoParameterCommandBase
	{
		private readonly IRepositoryProvider _provider;
		private readonly IDebugService _debugService;

		public DebugDumpImportantLinks(IRepositoryProvider provider, IDebugService debugService)
		{
			_provider = provider;
			_debugService = debugService;
		}

		protected override void Execute()
		{
			using (var session = _provider.Open<Task>())
			{
				var tasks = session.FindAll().ToList();

				if (tasks.Any())
				{
					var dump = tasks.Select(DumpImportantLinks).Aggregate((s1, s2) => s1 + Environment.NewLine + s2);

					_debugService.Dump(dump);
				}
			}
			
		}


		private static string DumpImportantLinks(Task task)
		{
			return task.Title + " LessImportantTasks: " + DumpLinks(task.LessImportantTasks) +
			       Environment.NewLine +
			       task.Title + " MoreImportantTasks: " + DumpLinks(task.MoreImportantTasks);
		}

		private static string DumpLinks(IEnumerable<Task> lessImportantTasks)
		{
			return lessImportantTasks.Any() 
				? lessImportantTasks.Select(t => t.Title).Aggregate((s1, s2) => s1 + ", " + s2)
				: String.Empty;
		}
	}
}
