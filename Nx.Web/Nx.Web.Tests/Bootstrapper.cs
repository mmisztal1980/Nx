using Ninject;
using Nx.Bootstrappers;

namespace Nx.Web.Tests
{
    public class Bootstrapper : BootstrapperBase
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
