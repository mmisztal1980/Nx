using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nx.Mongo
{
    public class MongoContextFactory : IMongoContextFactory
    {
        private bool _disposed;
        private const string DisposedErrorMessage = "The MongoContextFactory has already been disposed";

        public MongoContextFactory()
        {
        }

        ~MongoContextFactory()
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

        public TMongoContext CreateContext<TMongoContext>()
            where TMongoContext : class, IMongoContext
        {
            return CreateContext<TMongoContext>(null);
        }

        public TMongoContext CreateContext<TMongoContext>(string connectionStringName)
            where TMongoContext : class, IMongoContext
        {
            if (_disposed)
                throw new ObjectDisposedException(DisposedErrorMessage);

            return (TMongoContext)Activator.CreateInstance(typeof(TMongoContext), connectionStringName);
        }
    }
}