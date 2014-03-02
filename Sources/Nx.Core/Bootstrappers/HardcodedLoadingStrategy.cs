using Ninject;
using Ninject.Modules;

namespace Nx.Bootstrappers
{
    /// <summary>
    /// This strategy loads the modules passed as an array into the Kernel. 
    /// Useful in scenarios where the platform doesn't allow to load assemblies, such as WP7+
    /// </summary>
    public sealed class HardcodedLoadingStrategy : IModuleLoadingStrategy
    {
        public HardcodedLoadingStrategy(params INinjectModule[] modules)
        {
            this.modules = modules;
        }

        public void LoadModules(IKernel kernel)
        {
            if (modules != null)
            {
                kernel.Load(modules);
            }
        }

        private readonly INinjectModule[] modules;
    }
}
