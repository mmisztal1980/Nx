﻿using Ninject;
using Ninject.Modules;
using Nx.Bootstrappers;

namespace Nx.Core.UnitTests
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper(params INinjectModule[] modules)
            : base(modules)
        {
        }

        protected override IKernel CreateContainer()
        {
            return new StandardKernel();
        }

        protected override void ConfigureContainer()
        {
        }
    }
}