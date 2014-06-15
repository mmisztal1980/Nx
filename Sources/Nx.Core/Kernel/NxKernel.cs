using Ninject;
using Ninject.Modules;
using Nx.Extensions;
using System;
using System.Linq;

namespace Nx.Kernel
{
    /// <summary>
    /// A modified kernel that always adds DynamicProxyModule for interception and ensures that all IDisposable modules are disposed
    /// along with the kernel
    /// </summary>
    public class NxKernel : StandardKernel
    {
        public NxKernel(params INinjectModule[] modules)
            : base(modules)
        {
        }

        public NxKernel(INinjectSettings settings, params INinjectModule[] modules)
            : base(settings, modules)
        {
        }

#if INTERCEPTION
        public NxKernel(params INinjectModule[] modules)
            : base(new NinjectSettings() { LoadExtensions = false },
                new INinjectModule[] { new DynamicProxyModule() }.Concat(modules).ToArray())
        {
        }

        public NxKernel(INinjectSettings settings, params INinjectModule[] modules)
            : base(settings,
                new INinjectModule[] { new DynamicProxyModule() }.Concat(modules).ToArray())
        {
        }
#endif

        public override void Dispose(bool disposing)
        {
            GetModules()
                .Cast<IDisposable>()
                .ForEach(disposable =>
                {
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                });

            base.Dispose(disposing);
        }
    }
}