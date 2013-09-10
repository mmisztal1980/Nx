using Ninject;
using System;

namespace Nx.Bootstrappers
{
    public interface IBootstrapper : IDisposable
    {
        IKernel Run();
        IBootstrapper ExtendBy(Action<IKernel> action);
        IBootstrapper ExtendBy(IBootstrapperExtension extension);
        IBootstrapper ExtendBy<TBootstrapperExtension>() where TBootstrapperExtension : IBootstrapperExtension, new();
    }
}
