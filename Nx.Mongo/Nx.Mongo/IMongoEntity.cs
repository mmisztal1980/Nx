using MongoDB.Bson;

namespace Nx.Mongo
{
    public interface IMongoEntity<T>
    {
        ObjectId Id { get; set; }
    }
}