using Ninject;
using NUnit.Framework;
using Nx.Bootstrappers;
using Nx.Kernel;

namespace Nx.Core.Tests
{
    [TestFixture]
    public class WhenUsingBootstrapperExtensions
    {
        [Test]
        public void ShouldExtendTheKernelUsingNewExtensionObject()
        {
            using (var bootstrapper = new Bootstrapper().ExtendBy(new TestExtension()))
            using (var kernel = bootstrapper.Run())
            {
                Assert.IsTrue(kernel.IsRegistered<TestType>());
            }
        }

        [Test]
        public void ShouldExtendTheKernelUsingGenerics()
        {
            using (var bootstrapper = new Bootstrapper().ExtendBy<TestExtension>())
            using (var kernel = bootstrapper.Run())
            {
                Assert.IsTrue(kernel.IsRegistered<TestType>());
            }
        }

        private class TestExtension : IBootstrapperExtension
        {

            public void Extend(IKernel kernel)
            {
                kernel.RegisterTypeIfMissing<TestType>(false);
            }

            public string Name
            {
                get { return "TestExtension"; }
            }
        }

        private sealed class TestType
        {
        }
    }
}
