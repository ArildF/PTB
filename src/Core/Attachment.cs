using System;

namespace Rogue.Ptb.Core
{
	public class Attachment
	{
		public virtual Guid Id { get; set; }
		public virtual string? ContentType { get; set; }
		public virtual byte[]? Content { get; set; }
		public virtual string? Name { get; set; }
		
		public virtual DateTime Created { get; set; } = DateTime.Now;
	}
}