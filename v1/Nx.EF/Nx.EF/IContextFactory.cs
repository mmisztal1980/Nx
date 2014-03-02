using System;
using System.Data.Entity;

namespace Nx.EF
{
    public interface IContextFactory : IDisposable
    {
        TContext CreateContext<TContext>()
            where TContext : DbContext;

        TContext CreateContext<TContext>(string connectionStringName)
            where TContext : DbContext;
    }
}