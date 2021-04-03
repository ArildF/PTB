using System;
using System.Collections.Generic;
using System.Windows.Threading;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Views;
using StructureMap;
using StructureMap.Configuration.DSL;
using System.Linq;
using NHibernate.Util;

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

		public void Bootstrap(string[] args)
		{
			var options = ParseOptions(args);

			Container.Configure(ce =>
				{
					ce.AddRegistry<UIRegistry>();
					ce.AddRegistry<CoreRegistry>();
					ce.AddRegistry<InfrastructureRegistry>();
					ce.For<Func<Dialog, DialogHost>>().Use<Func<Dialog, DialogHost>>(
						c => 
							uc => 
								Container.With(uc).With(c.GetInstance<IShellView>()).GetInstance<DialogHost>());
					ce.For<Options>().Singleton().Use(options);
				});

			// RxApp.GetFieldNameForPropertyNameFunc = propName => "_" + Char.ToLower(propName[0]) + propName.Substring(1);

			SetUpIdleHandler();

		}

		private void SetUpIdleHandler()
		{
			Action idle = () =>
				{
					var aggregator = _container.GetInstance<IEventAggregator>();
					aggregator.Publish<ApplicationIdle>();
				};
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, idle);
		}

		private Options ParseOptions(IEnumerable<string> args)
		{
			return new() {TaskboardPath = args.FirstOrDefault()};
		}

		public IShellView CreateShell()
		{
			var startables = Container.Model.GetAllPossible<IStartable>();
			foreach (var startable in startables)
			{
				startable.Start();
			}

			return Container.GetInstance<IShellView>();
		}

		public Action<Exception> TryResolveExceptionHandler()
		{
			return Container.TryGetInstance<Action<Exception>>();
		}
	}

}
