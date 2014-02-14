using Nx.Cloud.Configuration;
using Nx.Cloud.Tables;

namespace Nx.Cloud.Tests.Tables
{
    public class TestTableRepository : TableRepository<TestTableData>
    {
        public TestTableRepository(ICloudConfiguration config)
            : base(config, "testtable")
        {
        }
    }
}