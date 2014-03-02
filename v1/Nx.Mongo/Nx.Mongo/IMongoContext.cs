using MongoDB.Driver;
using System;

namespace Nx.Mongo
{
    public interface IMongoContext : IDisposable
    {
        void Initialize(string databaseName);

        MongoDatabase GetDatabase();
    }
}