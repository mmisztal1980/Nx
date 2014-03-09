using NCrunch.Framework;
using Ninject;
using NUnit.Framework;
using Nx.Interception;
using Nx.Kernel;
using Nx.Logging;

namespace Nx.Core.Tests.Interception
{
    [TestFixture]
    public class WhenUsingExtendedDispose
    {
        [Test]
        [ExclusivelyUses(InterceptFooMethodInterceptor.ResourceName)]
        public void DisposeTargetsShouldBeDisposed()
        {
            using (var kernel = new NxKernel())
            {
                kernel.Bind<ILogFactory>().To<LogFactory>().InSingletonScope();
                kernel.Bind<ILogger>().To<NullLogger>();
                kernel.Bind<DisposeInterceptionTestObject>().ToSelf();
                kernel.Bind<ExtendedDisposeInterceptor>().ToSelf().InSingletonScope();

                var test = kernel.Get<DisposeInterceptionTestObject>();
                test.Dispose();

                Assert.IsTrue(test.Disposed);
                Assert.IsNull(test.GetDisposableField());
                Assert.IsNull(test.DisposableProperty);
            }
        }
    }
}