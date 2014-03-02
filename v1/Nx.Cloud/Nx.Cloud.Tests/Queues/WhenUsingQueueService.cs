using NCrunch.Framework;
using Ninject;
using NUnit.Framework;
using Nx.Cloud.Queues;

namespace Nx.Cloud.Tests.Queues
{
    [Ignore]
    public class WhenUsingQueueService : CloudTestFixtureBase
    {
        private const string QueueResourceName = "queue";

        [SetUp]
        public void Setup()
        {
            using (var queueService = Kernel.Get<IQueueService<TestQueueItem>>())
            {
                queueService.Clear();
                Assert.AreEqual(0, queueService.Length);
            }
        }

        [Test]
        [ExclusivelyUses(QueueResourceName)]
        public void ShouldCreateAndDisposeTheQueueService()
        {
            using (var queueService = Kernel.Get<IQueueService<TestQueueItem>>())
            {
            }
        }

        [Test]
        [ExclusivelyUses(QueueResourceName)]
        public void ShouldEnqueueAndDequeueAnItem()
        {
            using (var queueService = Kernel.Get<IQueueService<TestQueueItem>>())
            {
                Assert.AreEqual(0, queueService.Length);

                var enqueuedItem = new TestQueueItem() { Data = 3 };
                queueService.Enqueue(enqueuedItem);

                Assert.AreEqual(1, queueService.Length);

                var dequeuedItem = queueService.Dequeue();

                Assert.NotNull(dequeuedItem);
                Assert.AreEqual(enqueuedItem.Data, dequeuedItem.Data);
                Assert.AreEqual(0, queueService.Length);
            }
        }
    }
}