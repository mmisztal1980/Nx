using Nx.Queries;
using System;
using System.Collections.Generic;

namespace Nx.Repositories
{
    public interface IRepository<TEntity> : IDisposable
    {
        TEntity GetById<TId>(TId id) where TId : IEquatable<TId>;

        TEntity GetSingle(IQuery<TEntity> query);

        int GetList(IQuery<TEntity> query, out IEnumerable<TEntity> result);
    }
}