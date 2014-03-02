using Ninject;
using Nx.Bootstrappers;
using Nx.Kernel;
using Nx.Logging;

namespace Nx.Extensions
{
    public class NxLoggingExtension : IBootstrapperExtension
    {
        public void Extend(IKernel kernel)
        {
            if (kernel.IsRegistered<ILogger>())
            {
                kernel.Unbind<ILogger>();
            }
#if NETFX_CORE
            kernel.Bind<ILogger>().To<MetroLogger>();
#else
            kernel.Bind<ILogger>().To<NLogLogger>();
#endif
        }

        public string Name
        {
            get { return "Nx.Logging Extension"; }
        }
    }
}
