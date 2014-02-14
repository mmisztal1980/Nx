using Nx.Cloud.Configuration;
using Nx.Cloud.Queues;

namespace Nx.Cloud.Tests.Queues
{
    public class TestQueueService : QueueService<TestQueueItem>
    {
        public TestQueueService(ICloudConfiguration config)
            : base(config, "testqueue")
        {
        }
    }
}
