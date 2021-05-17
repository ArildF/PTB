using System;
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable VirtualMemberCallInConstructor

namespace Rogue.Ptb.Core
{
	public class Link
	{
		public Link(Task task, Task linkTo, LinkType type)
		{
			Task = task;
			LinkTo = linkTo;
			Type = type;
		}

		protected Link()
		{
			
		}

		public virtual Task? LinkTo
		{
			get; protected internal set;
		}

		public virtual LinkType Type
		{
			get;
			protected internal set;
		}

		public virtual Guid Id { get; protected internal set; }
		public virtual Task? Task { get; protected internal set; }
	}
}
