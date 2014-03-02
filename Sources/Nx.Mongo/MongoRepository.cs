using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Nx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Nx.Mongo
{
    public abstract class MongoRepository<TMongoEntity> : IMongoRepository<TMongoEntity>
     where TMongoEntity : class, IMongoEntity<TMongoEntity>
    {
        protected readonly ILogger Logger; // Owns it
        private readonly IMongoContextFactory _contextFactory; // Owns it

        private IMongoContext _context;
        private MongoDatabase _database;
        private MongoCollection<TMongoEntity> _collection;

        protected string ConnectionStringName;

        public abstract string CollectionName { get; }

        protected MongoRepository(ILogFactory logFactory, IMongoContextFactory contextFactory, string loggerName)
        {
            Logger = logFactory.CreateLogger(loggerName);
            _contextFactory = contextFactory;
        }

        ~MongoRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _contextFactory.Dispose();
                Logger.Dispose();
            }
        }

        public bool Initialized { get; private set; }

        public void Initialize(string connectionStringName, string databaseName)
        {
            if (!Initialized)
            {
                ConnectionStringName = connectionStringName;

                _context = _contextFactory.CreateContext<MongoContext>(ConnectionStringName);
                _context.Initialize(databaseName);

                _database = _context.GetDatabase();
                _collection = _database.GetCollection<TMongoEntity>(CollectionName);

                Initialized = true;
                Logger.Debug("Repository initialized to : {0}", ConnectionStringName);
            }
            else
            {
                Logger.Warning("Repository already initialized, attempt ignored.");
            }
        }

        private void Execute(Action<MongoCollection<TMongoEntity>> action)
        {
            try
            {
                if (!Initialized)
                {
                    throw new InvalidOperationException("Attempted to use an uninitialized repository");
                }

                action(_collection);
            }
            catch (InvalidOperationException ex)
            {
                Logger.Error("Invalid operation : {0}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while executing a repository operation. {0}", ex.Message);
                throw;
            }
        }

        public ObjectId Save(TMongoEntity entity)
        {
            var result = ObjectId.Empty;

            Execute((col) =>
            {
                col.Insert(entity);

                Logger.Debug("Entity[{0}] saved", entity.Id);

                result = entity.Id;
            });

            return result;
        }

        public int Save(IEnumerable<TMongoEntity> entities, out IEnumerable<ObjectId> ids)
        {
            int result = 0;
            IEnumerable<ObjectId> _ids = null;

            Execute((col) =>
            {
                var mongoEntities = entities as TMongoEntity[] ?? entities.ToArray();
                var results = col.InsertBatch(mongoEntities);

                _ids = mongoEntities.Select(e => e.Id);
                result = mongoEntities.Length;

                Logger.Debug("{0} Entities saved", result);
            });

            ids = _ids;

            return result;
        }

        public void Update(TMongoEntity entity)
        {
            Execute((col) =>
            {
                col.Update(Query.EQ("_id", entity.Id),
                    MongoDB.Driver.Builders.Update.Replace(entity),
                           UpdateFlags.Upsert);
                Logger.Debug("Entity[{0}] updated", entity.Id);
            });
        }

        public int Update(IEnumerable<TMongoEntity> entities)
        {
            int result = 0;

            foreach (var entity in entities)
            {
                Update(entity);
                result++;
            }
            Logger.Debug("{0} Entities updated", result);

            return result;
        }

        public void Delete(TMongoEntity entity)
        {
            Execute((col) =>
            {
                col.Remove(Query.EQ("_id", entity.Id));
                Logger.Debug("Entity[{0}] deleted", entity.Id);
            });
        }

        public int Delete(IEnumerable<TMongoEntity> entities)
        {
            int result = 0;

            foreach (var entity in entities)
            {
                Delete(entity);
                result++;
            }

            Logger.Debug("{0} Entities deleted", result);

            return result;
        }

        public int Delete(Expression<Func<TMongoEntity, bool>> predicate)
        {
            IEnumerable<TMongoEntity> entities;
            Get(predicate, out entities);

            return Delete(entities);
        }

        public TMongoEntity GetOne(Expression<Func<TMongoEntity, bool>> predicate)
        {
            TMongoEntity result = null;

            Execute((col) =>
            {
                result = col.AsQueryable().Single(predicate);

                Logger.Debug("Entity[{0}] retrieved", result.Id);
            });

            return result;
        }

        public int Get(Expression<Func<TMongoEntity, bool>> predicate, out IEnumerable<TMongoEntity> result)
        {
            int count = 0;
            IEnumerable<TMongoEntity> queryResults = null;

            Execute((col) =>
            {
                queryResults = col.AsQueryable().Where(predicate).ToArray();
                count = queryResults.Count();

                Logger.Debug("{0} Entities retrieved", count);
            });

            result = queryResults;

            return count;
        }

        public int Get(Expression<Func<TMongoEntity, bool>> predicate, int skip, int take, out IEnumerable<TMongoEntity> result)
        {
            int count = 0;
            IEnumerable<TMongoEntity> queryResults = null;

            Execute((col) =>
            {
                queryResults = col.AsQueryable().Where(predicate).OrderBy(e => e.Id).Skip(skip).Take(take).ToArray();
                count = queryResults.Count();

                Logger.Debug("{0} Entities retrieved", count);
            });

            result = queryResults;
            return count;
        }

        public int Get(IEnumerable<ObjectId> ids, out IEnumerable<TMongoEntity> result)
        {
            int count = 0;
            IEnumerable<TMongoEntity> queryResults = null;

            Execute((col) =>
            {
                queryResults = col.AsQueryable().Where(e => ids.Contains(e.Id)).ToArray();
                count = queryResults.Count();

                Logger.Debug("{0} Entities retrieved", count);
            });

            result = queryResults;
            return count;
        }
    }
}