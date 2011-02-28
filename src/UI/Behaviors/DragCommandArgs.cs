using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.Ptb.UI.Behaviors
{
	public class DragCommandArgs
	{
		public object Dragged { get; private set; }

		public object DragTarget { get; private set; }

		public DragCommandArgs(object dragged, object dragTarget)
		{
			Dragged = dragged;
			DragTarget = dragTarget;
		}
	}
}
