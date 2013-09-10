using Ninject;
using NUnit.Framework;
using Nx.Bootstrappers;
using Nx.Kernel;
using Nx.Logging;

namespace Nx.Core.Tests
{
    public class WhenUsingTheBootstrapper
    {
        [Test]
        public void TheSequenceShouldCompleteWithoutThrowing()
        {
            Assert.DoesNotThrow(() =>
            {
                using (BootstrapperBase bootstrapper = new Bootstrapper())
                using (IKernel kernel = bootstrapper.Run())
                {
                }
            });
        }

        [Test]
        public void TheILoggerTypeShouldBeRegistered()
        {
            using (BootstrapperBase bootstrapper = new Bootstrapper())
            using (IKernel kernel = bootstrapper.Run())
            {
                Assert.IsTrue(kernel.IsRegistered<ILogger>());
            }            
        }
    }
}
