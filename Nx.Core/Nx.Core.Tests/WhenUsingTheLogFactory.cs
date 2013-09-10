using Ninject;
using NUnit.Framework;
using Nx.Bootstrappers;
using Nx.Kernel;
using Nx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nx.Core.Tests
{
    public class WhenUsingTheLogFactory
    {
        [Test]
        public void ShouldCreateALogger()
        {
            using (BootstrapperBase bootstrapper = new Bootstrapper())
            using (IKernel kernel = bootstrapper.Run())
            {
                Assert.IsTrue(kernel.IsRegistered<ILogger>());
            }
        }

        [Test]
        [RequiresMTA]
        public void ShouldCreateLoggersThreadSafe()
        {
            using (BootstrapperBase bootstrapper = new Bootstrapper())
            using (IKernel kernel = bootstrapper.Run())
            {
                Task[] tasks = new Task[100];
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
