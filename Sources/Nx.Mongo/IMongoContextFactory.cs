using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nx.Mongo
{
    public interface IMongoContextFactory : IDisposable
    {
        TMongoContext CreateContext<TMongoContext>(string connectionStringName)
            where TMongoContext : class, IMongoContext;
    }
}