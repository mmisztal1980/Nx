using CommonServiceLocator.NinjectAdapter;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using Nx.Bootstrappers;
using Nx.Kernel;

namespace Nx.Extensions
{
    public class NxServiceLocatorExtension : IBootstrapperExtension
    {
        public void Extend(IKernel kernel)
        {
            if (!kernel.IsRegistered<IServiceLocator>())
            {
                kernel.Bind<IServiceLocator>().To<NinjectServiceLocator>().InSingletonScope();
                ServiceLocator.SetLocatorProvider(() => kernel.Get<IServiceLocator>());
            }
        }

        public string Name
        {
            get { return "Nx.ServiceLocator Extension"; }
        }
    }
}
