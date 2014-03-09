using NCrunch.Framework;
using Ninject;
using NUnit.Framework;
using Nx.Kernel;

namespace Nx.Core.Tests.Interception
{
    [TestFixture]
    public class WhenUsingMethodInterceptor
    {
        [Test]
        [ExclusivelyUses(InterceptFooMethodInterceptor.ResourceName)]
        public void FooShouldBeIntercepted()
        {
            using (var kernel = new NxKernel())
            {
                kernel.Bind<FooMethodInterceptionTestObject>().ToSelf();
                kernel.Bind<InterceptFooMethodInterceptor>().ToSelf().InSingletonScope();

                var test = kernel.Get<FooMethodInterceptionTestObject>();
                var interceptor = kernel.Get<InterceptFooMethodInterceptor>();
                test.Foo();

                Assert.IsTrue(interceptor.DidRun);
            }
        }

        [Test]
        [ExclusivelyUses(InterceptFooMethodInterceptor.ResourceName)]
        public void BarShouldNotBeIntercepted()
        {
            using (var kernel = new NxKernel())
            {
                kernel.Bind<FooMethodInterceptionTestObject>().ToSelf();
                kernel.Bind<InterceptFooMethodInterceptor>().ToSelf().InSingletonScope();

                var test = kernel.Get<FooMethodInterceptionTestObject>();
                var interceptor = kernel.Get<InterceptFooMethodInterceptor>();
                test.Bar();

                Assert.IsFalse(interceptor.DidRun);
            }
        }
    }
}