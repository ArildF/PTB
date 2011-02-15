using System;
using System.Diagnostics;
using System.Windows.Controls;
using ReactiveUI;
using Rogue.Ptb.Core;
using Rogue.Ptb.Infrastructure;
using Rogue.Ptb.UI.Views;
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
					ce.For<Func<Dialog, DialogHost>>().Use(c => 
						uc => Container.With(uc).With(c.GetInstance<IShellView>()).GetInstance<DialogHost>());

				});

			Debug.WriteLine(Container.WhatDoIHave());
			RxApp.GetFieldNameForPropertyNameFunc = propName => "_" + Char.ToLower(propName[0]) + propName.Substring(1);

		}

		public IShellView CreateShell()
		{
			var startables = Container.Model.GetAllPossible<IStartable>();
			startables.ForEach(startup => startup.Start());

			return Container.GetInstance<IShellView>();
		}
	}

}
