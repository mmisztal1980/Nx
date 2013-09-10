using Ninject;

namespace Nx.Bootstrappers
{
    public interface IModuleLoadingStrategy
    {
        void LoadModules(IKernel kernel);
    }
}
