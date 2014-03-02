using Ninject;
using NUnit.Framework;
using Nx.Extensions;
using Nx.Kernel;

namespace Nx.Logging.Tests
{
    [TestFixture]
    public class WhenUsingTheNxLoggingExtension
    {
        [Test]
        public void NLogLoggerShouldBeRegistered()
        {
            using (var bootstrapper = new Bootstrapper()
                .ExtendBy<NxLoggingExtension>())
            using (var kernel = bootstrapper.Run())
            {
                Assert.IsTrue(kernel.IsRegistered<ILogger>());
                var logger = kernel.Get<ILogger>();
                Assert.IsTrue(logger is NLogLogger);
            }
        }

        [Test]
        public void NLogLoggerShouldBeUsable()
        {
            using (var bootstrapper = new Bootstrapper()
                .ExtendBy<NxLoggingExtension>())
            using (var kernel = bootstrapper.Run())
            {
                var logger = kernel.Get<ILogger>();
                Assert.IsNotNull(logger);

                Assert.DoesNotThrow(() =>
                    {
                        logger.Debug("This is a DEBUG message");
                        logger.Info("This is a INFO message");
                        logger.Warning("This is a WARNING message");
                        logger.Error("This is a ERROR message");
                        logger.Fatal("This is a FATAL message");
                    });
            }
        }
    }
}
