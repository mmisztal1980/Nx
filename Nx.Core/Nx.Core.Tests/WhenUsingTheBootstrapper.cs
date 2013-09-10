using Ninject;
using NUnit.Framework;
using Nx.Bootstrappers;
using Nx.Logging;

namespace Nx.Core.Tests
{
    public class WhenUsingTheBootstrapper
    {
        [Test]
        public void TheSequenceShouldCompleteWithoutThrowing()
        {
            using (BootstrapperBase bootstrapper = new Bootstrapper())
            using (IKernel kernel = bootstrapper.Run())
            {
            }
        }

        [Test]
        public void TheLogFactoryShouldCreateLoggers()
        {
            using (BootstrapperBase bootstrapper = new Bootstrapper())
            using (IKernel kernel = bootstrapper.Run())
            using (ILogger logger = kernel.Get<ILogFactory>().CreateLogger("TestLogger"))
            {
                Assert.IsNotNull(logger);
                logger.Debug("hello!");
            }
        }
    }
}
