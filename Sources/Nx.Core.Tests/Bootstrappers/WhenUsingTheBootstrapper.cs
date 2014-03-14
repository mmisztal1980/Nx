using NUnit.Framework;
using Nx.Kernel;
using Nx.Logging;

namespace Nx.Core.Tests.Bootstrappers
{
    public class WhenUsingTheBootstrapper
    {
        [Test]
        public void TheSequenceShouldCompleteWithoutThrowing()
        {
            Assert.DoesNotThrow(() =>
            {
                using (var bootstrapper = new Bootstrapper())
                using (var kernel = bootstrapper.Run())
                {
                }
            });
        }

        [Test]
        public void TheILoggerTypeShouldBeRegistered()
        {
            using (var bootstrapper = new Bootstrapper())
            using (var kernel = bootstrapper.Run())
            {
                Assert.IsTrue(kernel.IsRegistered<ILogger>());
            }
        }
    }
}