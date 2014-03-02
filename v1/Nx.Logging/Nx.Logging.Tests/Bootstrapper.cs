using Ninject;
using Nx.Bootstrappers;

namespace Nx.Logging.Tests
{
    public sealed class Bootstrapper : BootstrapperBase
    {
        protected override void ConfigureContainer()
        {
        }

        protected override IKernel CreateContainer()
        {
            return new StandardKernel();
        }
    }
}
