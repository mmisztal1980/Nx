using Ninject;
using Nx.Bootstrappers;

namespace Nx.Core.Tests
{
    public class Bootstrapper : BootstrapperBase
    {
        protected override IKernel CreateContainer()
        {
            return new StandardKernel();
        }

        protected override void ConfigureContainer()
        {

        }
    }
}
