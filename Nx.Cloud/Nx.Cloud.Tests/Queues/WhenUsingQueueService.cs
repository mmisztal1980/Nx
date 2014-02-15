using Ninject;
using NUnit.Framework;
using Nx.Cloud.Queues;

namespace Nx.Cloud.Tests.Queues
{
    [Ignore]
    public class WhenUsingQueueService : CloudTestFixtureBase
    {
        public WhenUsingQueueService()
        {
            using (IQueueService<TestQueueItem> queueService = Kernel.Get<IQueueService<TestQueueItem>>())
            {
                queueService.Clear();
                Assert.AreEqual(0, queueService.Length);
            }
        }

        [Test]
        public void ShouldCreateAndDisposeTheQueueService()
        {
            Assert.DoesNotThrow(() =>
            {
                using (IQueueService<TestQueueItem> queueService = Kernel.Get<IQueueService<TestQueueItem>>())
                {
                }
            });
        }

        [Test]
        public void ShouldEnqueueAndDequeueAnItem()
        {
            Assert.DoesNotThrow(() =>
            {
                using (IQueueService<TestQueueItem> queueService = Kernel.Get<IQueueService<TestQueueItem>>())
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
            });
        }
    }
}