using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.Ptb.Core
{
	public class Task
	{
		public virtual Guid Id { get; private set; }
		public virtual string Title { get; set; }

		public virtual TaskState State
		{
			get; set;
		}
	}
}
