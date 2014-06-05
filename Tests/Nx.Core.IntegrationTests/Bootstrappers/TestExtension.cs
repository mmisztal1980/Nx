using Ninject;
using Nx.Bootstrappers;
using Nx.Kernel;

namespace Nx.Core.Tests.Bootstrappers
{
    internal sealed class TestExtension : IBootstrapperExtension
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
}