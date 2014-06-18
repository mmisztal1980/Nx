using NHibernate;
using NHibernate.Linq;
using Nx.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nx.Repositories
{
    public class NHibernateRepository<TEntity> : Disposable, IRepository<TEntity>
        where TEntity : class, IEntity
    {
        /// <summary>
        /// The NHibernate Session. The NHibernateRepository does not own the session
        /// </summary>
        private ISession _session;

        protected NHibernateRepository(ISession session)
        {
            _session = session;
        }

        public TEntity GetById<TId>(TId id) where TId : IEquatable<TId>
        {
            return _session.Get<TEntity>(id);
        }

        public TEntity GetSingle(IQuery<TEntity> query)
        {
            return query.SatisfyingElementFrom(_session.Query<TEntity>());
        }

        public int GetList(IQuery<TEntity> query, out IEnumerable<TEntity> result)
        {
            result = query.SatisfyingElementsFrom(_session.Query<TEntity>());
            return result.Count();
        }

        protected override void Dispose(bool disposing)
        {
            _session = null;
        }
    }
}