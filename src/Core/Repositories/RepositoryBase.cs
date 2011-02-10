﻿using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Extensions = Rogue.Ptb.Infrastructure.Extensions;

namespace Rogue.Ptb.Core.Repositories
{
	public class RepositoryBase<T> : IRepository<T>
	{
		private readonly ISession _session;

		public RepositoryBase(ISession session)
		{
			_session = session;
		}

		public IQueryable<T> FindAll()
		{
			return _session.Query<T>();
		}

		public void SaveAll(IEnumerable<T> items)
		{
			using (var tx = _session.BeginTransaction())
			{
				Extensions.ForEach(items.Cast<object>(), _session.SaveOrUpdate);
				tx.Commit();
			}
		}

		public void Save(T item)
		{
			SaveAll(new []{item});
		}

		public void Dispose()
		{
			_session.Flush();
			_session.Close();
		}
	}
}