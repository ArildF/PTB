using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;
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
				foreach (var item in items.Cast<object>())
				{
					_session.SaveOrUpdate(item);
				}
				tx.Commit();
			}
		}

		public void Save(T item)
		{
			SaveAll(new []{item});
		}

		public void InsertNew(T item)
		{
			using (var tx = _session.BeginTransaction())
			{
				_session.Save(item);
				tx.Commit();
			}
		}

		public void MergeAll(IEnumerable<T> items)
		{
			using (var tx = _session.BeginTransaction())
			{
				foreach (var item in items)
				{
					_session.SaveOrUpdate(item);
				}
				
				tx.Commit();
			}
		}

		public void Dispose()
		{
			_session.Flush();
			_session.Close();
		}
	}
}
