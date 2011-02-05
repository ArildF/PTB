using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StructureMap;

namespace Rogue.Ptb.Core
{
	public class RepositoryProvider : IRepositoryProvider
	{
		private readonly IContainer _container;

		public RepositoryProvider(IContainer container)
		{
			_container = container;
		}


		public IRepository<T> Open<T>()
		{
			return _container.GetInstance<IRepository<T>>();
		}
	}
}
