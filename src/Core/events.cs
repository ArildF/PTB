using System;

namespace Rogue.Ptb.Core
{
	public class DatabaseChanged
	{
		public string Path { get; private set; }

		public DatabaseChanged(string path)
		{
			Path = path;
		}
	}
	
}
