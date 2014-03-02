using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Nx.Mongo
{
    public abstract class MongoEntity<T> : IMongoEntity<T>
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}