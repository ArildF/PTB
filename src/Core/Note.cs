using System;

namespace Rogue.Ptb.Core
{
	public class Note
	{
		public virtual Guid Id { get; protected internal set; }
		public virtual string Markdown { get; set; }
		public virtual DateTime Created { get; set; }
		public virtual DateTime Modified { get; set; }
	}
}