using System;
using System.Collections.Generic;

namespace Rogue.Ptb.Core
{
	public class Note
	{
		public virtual Guid Id { get; protected internal set; }
		public virtual string Markdown { get; set; }
		public virtual DateTime Created { get; set; }
		public virtual DateTime Modified { get; set; }
		
		public virtual ISet<Attachment> Attachments { get; protected internal set; } = new HashSet<Attachment>();

		public virtual Attachment AddAttachment(Attachment attachment)
		{
			Attachments.Add(attachment);
			return attachment;
		}
	}
}