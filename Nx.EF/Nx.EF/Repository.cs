using Nx.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Nx.EF
{
#warning TODO : Consider using a single context for the duration of repository's lifetime

    public abstract class Repository<TContext, TEntity, TId> : IRepository<TEntity, TId>
        where TContext : DbContext
        where TEntity : class, IEntity<TId>
        where TId : IEquatable<TId>
    {
        private readonly IContextFactory _contextFactory;   // Owns it
        protected readonly ILogger Logger;                  // Owns it
        protected string ConnectionStringName;

        protected abstract Func<TContext, DbSet<TEntity>> SourceSelector { get; }

        protected Repository(ILogFactory logFactory, IContextFactory contextFactory, string loggerName)
            : this(logFactory.CreateLogger(loggerName), contextFactory)
        {
        }

        protected Repository(ILogger logger, IContextFactory contextFactory)
        {
            Initialized = false;
            Logger = logger;
            _contextFactory = contextFactory;
            Logger.Debug("Repository created");
        }

        #region IDisposable Members

        ~Repository()
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
            Logger.Debug("Repository destroyed");

            if (disposing)
            {
                OnDisposing();

                Logger.Dispose();
                _contextFactory.Dispose();
            }
        }

        protected virtual void OnDisposing()
        {
        }

        #endregion IDisposable Members

        public bool Initialized { get; private set; }

        public void Initialize(string connectionStringName)
        {
            if (!Initialized)
            {
                ConnectionStringName = connectionStringName;

                Initialized = true;
                Logger.Debug("Repository initialized to : {0}", ConnectionStringName);
            }
            else
            {
                Logger.Warning("Repository already initialized, attempt ignored.");
            }
        }

        private void Execute(Action<TContext> action)
        {
            try
            {
                if (!Initialized)
                {
                    throw new InvalidOperationException("Attempted to use an uninitialized repository");
                }

                using (var ctx = _contextFactory.CreateContext<TContext>(ConnectionStringName))
                {
                    action(ctx);
                }
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

        public TId Save(TEntity entity)
        {
            Execute((ctx) =>
            {
                var dbSet = SourceSelector(ctx);

                dbSet.Attach(entity);
                dbSet.Add(entity);
                ctx.SaveChanges();

                Logger.Debug("Entity[{0}] saved", entity.Id);
            });

            return entity.Id;
        }

        public int Save(IEnumerable<TEntity> entities, out IEnumerable<TId> ids)
        {
            int result = 0;

            Execute((ctx) =>
            {
                var dbSet = SourceSelector(ctx);

                foreach (var obj in entities)
                {
                    dbSet.Add(obj);
                    result++;
                }

                ctx.SaveChanges();

                Logger.Debug("{0} Entities saved", result);
            });

            ids = entities.Select(e => e.Id).ToArray();

            return result;
        }

        public void Update(TEntity entity)
        {
            Execute((ctx) =>
            {
                var original = SourceSelector(ctx).Single(e => e.Id.Equals(entity.Id));
                ctx.Entry(original).CurrentValues.SetValues(entity);

                ctx.SaveChanges();

                Logger.Debug("Entity {0} updated", entity.Id);
            });
        }

        public int Update(IEnumerable<TEntity> entities)
        {
            int result = 0;

            Execute((ctx) =>
            {
                var dbSet = SourceSelector(ctx);

                foreach (var entity in entities)
                {
                    var original = SourceSelector(ctx).Single(e => e.Id.Equals(entity.Id));
                    ctx.Entry(original).CurrentValues.SetValues(entity);

                    Logger.Debug("Updating entity {0}", entity.Id);
                    result++;
                }

                ctx.SaveChanges();

                Logger.Debug("Updated {0} entites", result);
            });

            return result;
        }

        public void Delete(TEntity entity)
        {
            Execute((ctx) =>
            {
                var dbSet = SourceSelector(ctx);
                dbSet.Attach(entity);
                dbSet.Remove(entity);
                ctx.SaveChanges();

                Logger.Debug("Entity[{0}] deleted", entity.Id);
            });
        }

        public int Delete(IEnumerable<TEntity> entities)
        {
            int result = 0;

            Execute((ctx) =>
            {
                var dbSet = SourceSelector(ctx);

                foreach (var entity in entities)
                {
                    dbSet.Attach(entity);
                    dbSet.Remove(entity);
                    Logger.Debug("Entity {0} deleted", entity.Id);
                    result++;
                }

                ctx.SaveChanges();

                Logger.Debug("{0} Entities deleted", result);
            });

            return result;
        }

        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            int result = 0;

            Execute((ctx) =>
            {
                var dbSet = SourceSelector(ctx);
                IEnumerable<TEntity> entities = dbSet.Where(predicate).ToArray();

                foreach (var entity in entities)
                {
                    dbSet.Attach(entity);
                    dbSet.Remove(entity);
                    Logger.Debug("Entity[{0}] deleted", entity.Id);
                    result++;
                }

                ctx.SaveChanges();

                Logger.Debug("{0} Entities deleted", result);
            });

            return result;
        }

        public TEntity GetOne(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity entity = null;

            Execute((ctx) =>
            {
                entity = SourceSelector(ctx).Single(predicate);

                Logger.Debug("Entity[{0}] retrieved", entity.Id);
            });

            return entity;
        }

        public int Get(Expression<Func<TEntity, bool>> predicate, out IEnumerable<TEntity> result)
        {
            int count = 0;
            IEnumerable<TEntity> entities = null;

            Execute((ctx) =>
            {
                entities = SourceSelector(ctx).Where(predicate).ToArray();
                count = entities.Count();

                Logger.Debug("{0} Entities retrieved", count);
            });

            result = entities;
            return count;
        }

        public int Get(Expression<Func<TEntity, bool>> predicate, int skip, int take, out IEnumerable<TEntity> result)
        {
            int count = 0;
            IEnumerable<TEntity> entities = null;

            Execute((ctx) =>
            {
                entities = SourceSelector(ctx).Where(predicate).OrderBy(e => e.Id).Skip(skip).Take(take).ToArray();
                count = entities.Count();

                Logger.Debug("{0} Entities retrieved", count);
            });

            result = entities;

            return count;
        }

        public int Get(IEnumerable<TId> ids, out IEnumerable<TEntity> result)
        {
            int count = 0;
            IEnumerable<TEntity> entities = null;

            Execute((ctx) =>
            {
                entities = SourceSelector(ctx).Where(e => ids.Contains(e.Id)).ToArray();
                count = entities.Count();

                Logger.Debug("{0} Entities retrieved", count);
            });

            result = entities;
            return count;
        }
    }
}