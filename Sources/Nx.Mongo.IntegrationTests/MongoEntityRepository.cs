using Nx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nx.Mongo.IntegrationTests
{
    public class MongoEntityRepository : MongoRepository<MongoEntity>, IMongoEntityRepository
    {
        public MongoEntityRepository(ILogFactory logFactory, IMongoContextFactory contextFactory)
            : base(logFactory, contextFactory, "TestMongoEntityRepository")
        {
        }

        public override string CollectionName
        {
            get { return "TestEntities"; }
        }
    }
}