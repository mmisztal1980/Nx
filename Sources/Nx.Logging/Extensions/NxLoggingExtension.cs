using Ninject;
using Nx.Bootstrappers;
using Nx.Kernel;
using Nx.Logging;

// ReSharper disable CheckNamespace
namespace Nx.Extensions
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// This extension replaces the default NullLoger binding in the Kernel with the one from this assembly.
    /// </summary>
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