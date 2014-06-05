using Ninject;
using NUnit.Framework;
using Nx.Kernel;
using Nx.Logging;
using System;
using System.Threading.Tasks;

namespace Nx.Core.UnitTests.Logging
{
    [TestFixture]
    public class WhenUsingTheLogFactory
    {
        [Test]
        public void ShouldCreateALogger()
        {
            using (var bootstrapper = new Bootstrapper())
            using (var kernel = bootstrapper.Run())
            {
                Assert.IsTrue(kernel.IsRegistered<ILogger>());
            }
        }

        [Test]
        [RequiresMTA]
        public void ShouldCreateLoggersThreadSafe()
        {
            using (var bootstrapper = new Bootstrapper())
            using (var kernel = bootstrapper.Run())
            {
                var tasks = new Task[100];
                for (int i = 0; i < tasks.Length; i++)
                {
                    tasks[i] = new Task(X, kernel.Get<ILogFactory>());
                }

                foreach (var task in tasks)
                {
                    task.Start();
                }

                Task.WaitAll(tasks);
            }
        }

        public static void X(object o)
        {
            var logFactory = o as ILogFactory;
            Assert.IsNotNull(logFactory);
            using (var logger = logFactory.CreateLogger(Guid.NewGuid().ToString()))
            {
                Assert.IsNotNull(logger);
                logger.Debug("logger created");
            }
        }
    }
}