using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nx.Mongo.IntegrationTests
{
    public class MongoEntity : MongoEntity<MongoEntity>
    {
        public string Name { get; set; }

        public string Property { get; set; }
    }
}