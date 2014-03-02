using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Nx.Mongo
{
    public interface IMongoRepository<TEntity> : IDisposable
       where TEntity : class, IMongoEntity<TEntity>
    {
        string CollectionName { get; }

        bool Initialized { get; }

        void Initialize(string connectionStringName, string databaseName);

        ObjectId Save(TEntity entity);

        int Save(IEnumerable<TEntity> entities, out IEnumerable<ObjectId> ids);

        void Update(TEntity entity);

        int Update(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        int Delete(IEnumerable<TEntity> entities);

        int Delete(Expression<Func<TEntity, bool>> predicate);

        TEntity GetOne(Expression<Func<TEntity, bool>> predicate);

        int Get(Expression<Func<TEntity, bool>> predicate, out IEnumerable<TEntity> result);

        int Get(Expression<Func<TEntity, bool>> predicate, int skip, int take, out IEnumerable<TEntity> result);

        int Get(IEnumerable<ObjectId> ids, out IEnumerable<TEntity> result);
    }
}