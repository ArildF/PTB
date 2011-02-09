using System;
using ReactiveUI;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;
using StructureMap;

namespace Rogue.Ptb.UI
{
	public class Bootstrapper
	{
		private readonly Container _container;

		public Bootstrapper()
		{
			_container = new Container();
		}

		public Container Container
		{
			get { return _container; }
		}

		public void Bootstrap()
		{
			Container.Configure(ce =>
				{
					ce.AddRegistry<UIRegistry>();
					ce.AddRegistry<CoreRegistry>();
					ce.AddRegistry<InfrastructureRegistry>();
				});

			RxApp.GetFieldNameForPropertyNameFunc = propName => "_" + Char.ToLower(propName[0]) + propName.Substring(1);
		}

		public IShellView CreateShell()
		{
			return Container.GetInstance<IShellView>();
		}
	}

}
