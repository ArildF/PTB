using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.Ptb.Core.Export
{
	public class TaskDto
	{
		public Guid Id { get; set; }
		public string Title { get; set; }

		public TaskState State { get; set; }

		public TaskDto()
		{
			
		}
	}
}
