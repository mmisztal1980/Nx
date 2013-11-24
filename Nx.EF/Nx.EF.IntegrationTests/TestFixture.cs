using Ninject;
using NUnit.Framework;
using Nx.Kernel;
using Nx.Logging;

namespace Nx.EF.IntegrationTests
{
    public class TestFixture
    {
        // Not owned by TestFixture
        public IKernel Kernel { get; private set; }

        // Owned by TestFixture
        public ILogger Logger { get; private set; }

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            Kernel = AssemblySetup.Kernel;
            Assert.NotNull(Kernel);
            Assert.IsTrue(Kernel.IsRegistered<ILogFactory>());
            Logger = Kernel.Get<ILogFactory>().CreateLogger("Test");
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            Kernel = null;

            if (Logger != null)
            {
                Logger.Dispose();
            }
        }
    }
}