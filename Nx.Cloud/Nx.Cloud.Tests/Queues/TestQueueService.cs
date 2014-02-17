using Nx.Cloud.Configuration;
using Nx.Cloud.Queues;
using Nx.Logging;

namespace Nx.Cloud.Tests.Queues
{
    public class TestQueueService : QueueService<TestQueueItem>
    {
        public TestQueueService(ILogFactory logFactory, ICloudConfiguration config)
            : base(logFactory, config, "testqueue")
        {
        }
    }
}