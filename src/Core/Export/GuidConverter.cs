using System;
using Glue.Converters;

namespace Rogue.Ptb.Core.Export
{
	public class GuidConverter : BaseSimpleConverter<Guid, Guid>
	{
		public override Guid MapTowardsRight(Guid from)
		{
			return from;
		}

		public override Guid MapTowardsLeft(Guid from)
		{
			return from;
		}
	}
}