using MongoDB.Driver;
using System;
using System.Configuration;

namespace Nx.Mongo
{
    public class MongoContext : IMongoContext
    {
        private bool _isInitialized;
        private bool _disposed;
        private string _databaseName;
        private MongoClient _client;
        private MongoServer _server;
        private readonly string _connectionStringName;

        public MongoContext(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        ~MongoContext()
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

        public void Initialize(string databaseName)
        {
            DisposedCheck();

            if (!_isInitialized)
            {
                MongoDefaults.MaxConnectionIdleTime = TimeSpan.FromMinutes(1);
                _client = new MongoClient(ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString);
                _server = _client.GetServer();

                _databaseName = databaseName;
                _isInitialized = true;
            }
        }

        public MongoDatabase GetDatabase()
        {
            DisposedCheck();

            return _server.GetDatabase(_databaseName);
        }

        private void DisposedCheck()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("An attempt was made to use a disposed MongoContext");
            }
        }
    }
}