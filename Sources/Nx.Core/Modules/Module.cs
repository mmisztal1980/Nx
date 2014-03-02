using Ninject;
using Ninject.Modules;
using Nx.Logging;
using System;
using System.Globalization;

namespace Nx.Modules
{
    public abstract class Module : NinjectModule, IDisposable
    {
        public Module()
        {
        }

        #region IDisposable Members
        ~Module()
        {
            Dispose(false);
        }

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected new void Dispose(bool disposing)
        {
            if (disposing)
            {
                OnDisposing();
            }

            base.Dispose(disposing);
        }
        #endregion

        public override void Load()
        {
            ModuleLogger = this.Kernel.Get<ILogFactory>().CreateLogger(string.Format(CultureInfo.InvariantCulture, "NxModule:{0}", Name));

            ModuleLogger.Info("Loading...");
            OnLoading();
        }

        public new abstract string Name { get; }

        public ILogger ModuleLogger { get; private set; }

        public abstract void OnDisposing();

        public abstract void OnLoading();
    }
}
