using Ninject;
using Ninject.Parameters;
using System;

namespace Nx.Logging
{
    public class LogFactory : ILogFactory
    {
        private IKernel _kernel;
        private static readonly object Lock = new object();

        public LogFactory(IKernel kernel)
        {
            this._kernel = kernel;
        }

        ~LogFactory()
        {
            _kernel = null;
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
                _kernel = null;
            }
        }

        public ILogger CreateLogger(string loggerName)
        {
            lock (Lock)
            {
                Condition.ArgumentNotNullOrEmpty(loggerName, "loggerName");
                return _kernel.Get<ILogger>(new ConstructorArgument("loggerName", loggerName));
            }
        }

        public ILogger CreateLogger(object loggerOwner)
        {
            lock (Lock)
            {
                Condition.ArgumentNotNull(loggerOwner, "loggerOwner");
                return _kernel.Get<ILogger>(new ConstructorArgument("loggerName", loggerOwner.GetType().Name));
            }
        }
    }
}
