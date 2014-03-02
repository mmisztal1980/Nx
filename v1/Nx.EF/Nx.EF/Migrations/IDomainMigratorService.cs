using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace Nx.EF.Migrations
{
    public interface IDomainMigratorService : IDisposable
    {
        /// <summary>
        /// Migrates the database from an EF6 code-first model
        /// </summary>
        /// <typeparam name="TContext">The Context type</typeparam>
        /// <typeparam name="TConfiguration">The DbMigrationConfiguration type</typeparam>
        /// <returns>true if a migration did take place, false if no changes to the database were made</returns>
        bool Migrate<TContext, TConfiguration>()
            where TContext : DbContext
            where TConfiguration : DbMigrationsConfiguration<TContext>, new();

        /// <summary>
        /// Migrates the database from an EF6 model
        /// </summary>
        /// <typeparam name="TContext">The Context type</typeparam>
        /// <typeparam name="TConfiguration">The DbMigrationConfiguration type</typeparam>
        /// <param name="connectionStringName">connection string or connection string's name</param>
        /// <returns>true if a migration did take place, false if no changes to the database were made</returns>
        bool Migrate<TContext, TConfiguration>(string connectionStringName)
            where TContext : DbContext
            where TConfiguration : DbMigrationsConfiguration<TContext>, new();

        //bool Migrate<TContext, TConfiguration>(DomainConfigurationSection config)
        //    where TContext : DbContext
        //    where TConfiguration : DbMigrationsConfiguration<TContext>, new();

        /// <summary>
        /// Drops the database
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <returns></returns>
        bool Drop<TContext>()
            where TContext : DbContext;

        /// <summary>
        /// Drops the database
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="connectionStringName"></param>
        /// <returns></returns>
        bool Drop<TContext>(string connectionStringName)
            where TContext : DbContext;

        //bool Drop<TContext>(DomainConfigurationSection config)
        //    where TContext : DbContext;
    }
}