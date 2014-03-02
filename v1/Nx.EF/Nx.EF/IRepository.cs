using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nx.EF
{
    public interface IRepository<TEntity, TId> : IDisposable
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {
        bool Initialized { get; }

        void Initialize(string connectionStringName);

        int Count();

        int Count(Expression<Func<TEntity, bool>> predicate);

        TId Save(TEntity entity);

        int Save(IEnumerable<TEntity> entities, out IEnumerable<TId> ids);

        void Update(TEntity entity);

        int Update(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        int Delete(IEnumerable<TEntity> entities);

        int Delete(Expression<Func<TEntity, bool>> predicate);

        TEntity GetOne(Expression<Func<TEntity, bool>> predicate);

        int Get(Expression<Func<TEntity, bool>> predicate, out IEnumerable<TEntity> result);

        int Get<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy,
            bool descending, out IEnumerable<TEntity> result);

        int Get(Expression<Func<TEntity, bool>> predicate, int skip, int take, out IEnumerable<TEntity> result);

        int Get<TKey>(Expression<Func<TEntity, bool>> predicate, int skip, int take,
            Expression<Func<TEntity, TKey>> orderBy, bool descending, out IEnumerable<TEntity> result);

        int Get(IEnumerable<TId> ids, out IEnumerable<TEntity> result);
    }
}