using NUnit.Framework;
using Nx.Cloud.Concurrency;

namespace Nx.Cloud.Tests.Concurrency
{
    [Ignore]
    public class WhenUsingCloudLock : CloudTestFixtureBase
    {
        [Test]
        public void ShouldAquireLockInSingleThreadAccessScenario()
        {
            Assert.DoesNotThrow(() =>
            {
                using (var @lock = new CloudLock("testlock"))
                {
                    Assert.True(@lock.HasLock);
                }
            });
        }

        [Test]
        public void ShouldExecuteAnActionUsingTheSyntax()
        {
            Assert.DoesNotThrow(() =>
            {
                bool flag = false;

                CloudLock.Named("testlock").Execute(() =>
                {
                    flag = true;
                });

                Assert.IsTrue(flag);
            });
        }

        [Test]
        public void ShouldExecuteAnActionWithParameterUsingTheSyntax()
        {
            Assert.DoesNotThrow(() =>
            {
                bool flag = false;

                CloudLock.Named("testlock").Execute<bool>((param) =>
                {
                    flag = param;
                }, true);

                Assert.IsTrue(flag);
            });
        }
    }
}