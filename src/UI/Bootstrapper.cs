using Rogue.Ptb.Core;
using StructureMap;

namespace Rogue.Ptb.UI
{
	class Bootstrapper
	{
		public IShellView Bootstrap()
		{
			var container = new Container();

			container.Configure(ce => {
				ce.AddRegistry<UIRegistry>();
				ce.AddRegistry<CoreRegistry>();
			});

			return container.GetInstance<IShellView>();
		}
	}

}
