using Nx.Cloud.Configuration;
using Nx.Cloud.Tables;
using Nx.Logging;

namespace Nx.Cloud.Tests.Tables
{
    public class TestTableRepository : TableRepository<TestTableData>
    {
        public TestTableRepository(ILogFactory logFactory, ICloudConfiguration config)
            : base(logFactory, config, "testtable")
        {
        }
    }
}