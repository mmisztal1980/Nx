using System;
using System.Data.Entity;

namespace Nx.EF
{
    public class ContextFactory : IContextFactory
    {
        private bool _disposed = false;
        private const string DisposedErrorMessage = "The ContextFactory has already been disposed";

        public ContextFactory()
        {
        }

        ~ContextFactory()
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
                _disposed = true;
            }
        }

        public TContext CreateContext<TContext>()
            where TContext : DbContext
        {
            return CreateContext<TContext>(null);
        }

        public TContext CreateContext<TContext>(string connectionStringName)
            where TContext : DbContext
        {
            if (_disposed)
                throw new ObjectDisposedException(DisposedErrorMessage);

            if (connectionStringName != null && connectionStringName.StartsWith("name="))
            {
                connectionStringName = connectionStringName.Remove(0, 5);
            }

            return
                (TContext)
                (string.IsNullOrEmpty(connectionStringName)
                     ? Activator.CreateInstance(typeof(TContext))
                     : Activator.CreateInstance(typeof(TContext), connectionStringName));
        }
    }
}