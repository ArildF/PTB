using System;

namespace Rogue.Ptb.Core
{
	public class Link
	{
		public Link(Task linkTo, LinkType type)
		{
			LinkTo = linkTo;
			Type = type;
		}

		protected Link()
		{
			
		}

		public virtual Task LinkTo
		{
			get; private set;
		}

		public virtual LinkType Type
		{
			get; private set; 
		}

		public virtual Guid Id { get; private set; }
	}
}
