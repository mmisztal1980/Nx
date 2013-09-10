using Ninject;
using Ninject.Infrastructure;
using Ninject.Modules;
using Nx.Kernel;
using Nx.Logging;
using System;

namespace Nx.Bootstrappers
{
    /// <summary>
    /// The Bootstrapper. Executes the initialization sequence for the application. 
    /// </summary>
    public abstract class BootstrapperBase : IHaveKernel, IBootstrapper, IDisposable
    {
        protected readonly INinjectSettings kernelSettings = null;
        protected readonly IModuleLoadingStrategy ModuleLoadingStrategy = null;
        protected bool useDefaultConfiguration = true;
        protected const string LoggerName = "SYSTEM";

        public IKernel Kernel { get; protected set; }

        public BootstrapperBase(params INinjectModule[] modules)
        {
            ModuleLoadingStrategy = new HardcodedLoadingStrategy(modules);

            this.Kernel = this.CreateContainer();
            this.PreConfigureContainer();
            this.Kernel.Bind<ILogFactory>().To<LogFactory>().InSingletonScope();

            this.ConfigureLogging();
        }

        public BootstrapperBase(INinjectSettings settings, params INinjectModule[] modules)
            : this(modules)
        {
            kernelSettings = settings;
        }

        #region IDisposable Members
        ~BootstrapperBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                OnDisposing();
            }
        }

        public virtual void OnDisposing()
        {
        }
        #endregion

        public IKernel Run()
        {
            using (ILogger log = GetLogger(LoggerName))
            {
                log.Info("Starting the bootstrapper sequence...");

                log.Info("Configuring the container...");
                this.ConfigureContainer();

                log.Info("Loading modules...");
                this.LoadModules();

                return this.Kernel;
            }
        }

        public IBootstrapper ExtendBy(Action<IKernel> action)
        {
            using (ILogger log = GetLogger(LoggerName))
            {
                action(Kernel);
                log.Info("Applied custom action extension");
                return this;
            }
        }

        public IBootstrapper ExtendBy(IBootstrapperExtension extension)
        {
            using (ILogger log = GetLogger(LoggerName))
            {
                extension.Extend(this.Kernel);
                log.Info("Applied Bootstrapper Extension : {0}", extension.Name);
                return this;
            }
        }

        public IBootstrapper ExtendBy<TBootstrapperExtension>()
            where TBootstrapperExtension : IBootstrapperExtension, new()
        {
            return ExtendBy(new TBootstrapperExtension());
        }

        protected void LoadModules()
        {
            this.ModuleLoadingStrategy.LoadModules(Kernel);
        }

        /// <summary>
        /// Creates the <see cref="IKernel"/> that will be used as the default container.
        /// </summary>
        /// <returns>A new instance of <see cref="IKernel"/>.</returns>
        protected abstract IKernel CreateContainer();

        /// <summary>
        /// Ensures the IKernel is registered. Ensures the ServiceLocator is initialized
        /// </summary>
        protected virtual void PreConfigureContainer()
        {
            if (!Kernel.IsRegistered<IKernel>())
            {
                this.Kernel.Bind<IKernel>().ToConstant(this.Kernel).InSingletonScope();
            }
        }

        /// <summary>
        /// Configures the logging via the Nx.Logging.ILogger interface
        /// </summary>
        protected virtual void ConfigureLogging()
        {
            this.Kernel.Bind<ILogger>().To<NullLogger>().InTransientScope();
        }

        protected abstract void ConfigureContainer();

        private ILogger GetLogger(string loggerName)
        {
            return Kernel.Get<ILogFactory>().CreateLogger(loggerName);
        }
    }
}
