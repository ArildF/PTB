using System;
using ReactiveUI;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;
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
				ce.AddRegistry<InfrastructureRegistry>();
			});

			RxApp.GetFieldNameForPropertyNameFunc = propName => "_" + Char.ToLower(propName[0]) + propName.Substring(1);

			return container.GetInstance<IShellView>();
		}
	}

}
