using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nx.EF.IntegrationTests
{
    public interface ITestEntityRepository : IRepository<TestEntity, int>
    {
    }
}