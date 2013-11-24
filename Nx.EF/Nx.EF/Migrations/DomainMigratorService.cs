using Nx.Logging;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Nx.EF.Migrations
{
    /// <summary>
    /// The DomainMigrator is used to migrate the EF6 Context into the actual database
    /// </summary>
    public sealed class DomainMigratorService : IDomainMigratorService
    {
        private readonly ILogger _logger;
        private readonly IContextFactory _contextFactory;

        public DomainMigratorService(ILogFactory logFactory, IContextFactory contextFactory)
        {
            _logger = logFactory.CreateLogger("DomainMigrator");
            _contextFactory = contextFactory;
        }

        ~DomainMigratorService()
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
                _logger.Dispose();
                _contextFactory.Dispose();
            }
        }

        /// <summary>
        /// Migrates the database from an EF5 model
        /// </summary>
        /// <typeparam name="TContext">The Context type</typeparam>
        /// <typeparam name="TConfiguration">The DbMigrationConfiguration type</typeparam>
        /// <returns>true if a migration did take place, false if no changes to the database were made</returns>
        public bool Migrate<TContext, TConfiguration>()
            where TContext : DbContext
            where TConfiguration : DbMigrationsConfiguration<TContext>, new()
        {
            return Migrate<TContext, TConfiguration>(string.Empty);
        }

        /// <summary>
        /// Migrates the database from an EF5 model
        /// </summary>
        /// <typeparam name="TContext">The Context type</typeparam>
        /// <typeparam name="TConfiguration">The DbMigrationConfiguration type</typeparam>
        /// <param name="connectionStringName">connection string or connection string's name</param>
        /// <returns>true if a migration did take place, false if no changes to the database were made</returns>
        public bool Migrate<TContext, TConfiguration>(string connectionStringName)
            where TContext : DbContext
            where TConfiguration : DbMigrationsConfiguration<TContext>, new()
        {
            try
            {
                using (var context = _contextFactory.CreateContext<TContext>(connectionStringName))
                {
                    if (!context.Database.Exists())
                    {
                        _logger.Info("The database does not exist and will be created");

                        Database.SetInitializer(
                    !string.IsNullOrEmpty(connectionStringName)
                        ? new MigrateDatabaseToLatestVersion<TContext, TConfiguration>(connectionStringName)
                        : new MigrateDatabaseToLatestVersion<TContext, TConfiguration>());

                        context.Database.CreateIfNotExists();
                        return true;
                    }
                    else
                    {
                        _logger.Info("The database exists, no action neccessary");
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("An error occured while attempting to migrate the database : {0}", ex.Message);
                throw;
            }
        }

        //public bool Migrate<TContext, TConfiguration>(DomainConfigurationSection config)
        //    where TContext : DbContext
        //    where TConfiguration : DbMigrationsConfiguration<TContext>, new()
        //{
        //    return Migrate<TContext, TConfiguration>(config.SqlServerConnectionStringName);
        //}

        public bool Drop<TContext>() where TContext : DbContext
        {
            return Drop<TContext>(string.Empty);
        }

        public bool Drop<TContext>(string connectionStringName) where TContext : DbContext
        {
            try
            {
                using (var context = _contextFactory.CreateContext<TContext>(connectionStringName))
                {
                    if (!context.Database.Exists())
                    {
                        _logger.Debug("No database exists... aborting the drop");
                        return false;
                    }

                    _logger.Debug("Dropping the database...");
                    context.Database.Delete();
                    _logger.Debug("Database dropped");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error("An error occured while attempting to drop the database : {0}", ex.Message);
                throw;
            }
        }

        //public bool Drop<TContext>(DomainConfigurationSection config)
        //    where TContext : DbContext
        //{
        //    return Drop<TContext>(config.SqlServerConnectionStringName);
        //}
    }
}