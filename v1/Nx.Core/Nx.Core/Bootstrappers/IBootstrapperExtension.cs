using Ninject;

namespace Nx.Bootstrappers
{
    public interface IBootstrapperExtension
    {
        void Extend(IKernel kernel);
        string Name { get; }
    }
}
